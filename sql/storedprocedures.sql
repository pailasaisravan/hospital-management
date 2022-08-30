

alter procedure uspverifylogin   --1
( @id varchar(20),
 @password varchar(20)
)
as
begin
declare @uid int
if exists(select *  from tbl_login where loginid=@id and password = @password and status = 'active')
begin
select username,role_id,uid from tbl_login where loginid=@id and password = @password --2
select * from tbl_roomtype
select rid,roomtype,roomcode,price from tbl_rooms where status='available'
select did,doctorcode,doctorname,mobile,speciality from tbl_doctors where status ='available'
select * from tbl_rooms
select * from tbl_doctors
update tbl_login set last_login_date = GETDATE() where loginid=@id and password = @password

end
else
select 'invalid userid or password' as loginstatus
end



alter procedure uspcreatedoctor   --2
(@name nvarchar(100),
@mobile nvarchar(50),
@speciality nvarchar(50),
@fee float,
@loginid varchar(20),
@password varchar(20),
@username varchar(50)
)
as
begin
if not exists(select * from tbl_login where loginid = @loginid and password=@password )
begin
insert into tbl_login(loginid, password, username, role_id) values (@loginid,@password,@username,3)
declare @uid int
declare @did int
declare @dcode varchar(10)

set @uid = (select uid from tbl_login where loginid=@loginid and password = @password)
set @did= (select isnull(max(did),0)+1 from tbl_doctors)
set @dcode =( select  'DCT-' + right(REPLICATE('0',3)+cast(@did as varchar),3))
insert into tbl_doctors(doctorname,doctorcode, mobile, speciality, consultation_fee, edate, uid)
values(@name,@dcode,@mobile,@speciality,@fee,getdate(),@uid) 
end
end

alter procedure uspeditdoctor
(
@did int,
@name nvarchar(50),
@mobile nvarchar(50)
)
as
begin
if(@name='' and @mobile !='')
begin
update tbl_doctors set mobile=@mobile where did= @did
end
else if(@name!='' and @mobile ='')
begin
update tbl_doctors set doctorname=@name where did= @did
end
else if(@name!='' and @mobile !='')
begin
update tbl_doctors set doctorname=@name,mobile=@mobile where did= @did
end

end



create procedure uspdeletedoctor
(
@did int
)
as
begin

update tbl_doctors set status = 'unavailable' where did= @did
end




create procedure uspshowallspeciality
as
begin
select speciality_name from tbl_speciality
end








ALTER procedure uspaddroom
(
	@type nvarchar(50),
	@price float
)
as
begin
declare @rid  int
declare @roomcode nvarchar(50)
set @rid = (select isnull(count(rid),0)+1  from tbl_rooms where roomtype=@type)

if (@type = 'A.C Room') 
begin
set @roomcode = (select  'AC-' + cast(@rid as varchar))
end
ELSE if (@type = 'Special Room') 
begin
set @roomcode = (select  'SP-' + cast(@rid as varchar))
end
ELSE if (@type = 'Super deluxe Room') 
begin
set @roomcode = (select  'SDR-' + cast(@rid as varchar))
end
ELSE if (@type = 'I.C.U')
begin
set @roomcode = (select  'ICU-' + cast(@rid as varchar))
end

insert into tbl_rooms(roomtype, roomcode, price) values(@type,@roomcode,@price)
end


create procedure uspeditroom


alter procedure uspaddpatient
(
@name nvarchar(50),
@gender nvarchar(50),
@age int,
@address nvarchar(50),
@city nvarchar(50),
@pincode nvarchar(50),
@mobile nvarchar(50),
@ref nvarchar(50),
@disease nvarchar(50)
)
as
begin
insert into tbl_patient( name, gender, age, address, city, pincode, mobile, reference_doctor, disease)
values(@name,@gender,@age,@address,@city,@pincode,@mobile,@ref,@disease)

end


alter procedure usproomassign
(
@pid int,
@type nvarchar(50),
@rid int
)
as
begin
if exists(select * from tbl_patient where pid = @pid)
begin
update tbl_patient set room_type =@type where pid = @pid
update tbl_patient set status ='admitted' where pid = @pid
update tbl_patient set edate =GETDATE() where pid = @pid
update tbl_rooms set status ='occupied' where rid=@rid
update tbl_patient set rid=@rid where pid=@pid
end
end

alter procedure uspdoctorassign
(
@pid int,
@name nvarchar(50)
)
as
begin
if exists(select * from tbl_patient where pid = @pid)
begin
update tbl_patient set doctor_name=@name where pid = @pid
update tbl_doctors set status ='unavailable' where doctorname= @name
end
end

alter procedure uspgeneratebill
(
@pid int,
@paidbill float,
@medicinebill float
)
as
begin
if exists(select * from tbl_patient where pid = @pid)
begin
if not exists ( select * from tbl_bills where pid = @pid) 
begin
declare @roombill float
declare @doctorbill float
declare @totalbill float
declare @rembill float
declare @stat nvarchar(30)
set @roombill= (select top 1 r.price from tbl_patient as p   join tbl_rooms as r on r.roomtype = p.room_type  where p.pid = @pid)
set @doctorbill = (select d.consultation_fee from tbl_doctors as d join tbl_patient as p on p.doctor_name=d.doctorname where p.pid = @pid)
set @totalbill = isnull(@roombill,0)+isnull(@doctorbill,0)+isnull(@medicinebill,0)
set @rembill = @totalbill - @paidbill
if(@rembill=0)
begin
set @stat = 'paid'
end
else if(@rembill!=0)
begin
set @stat = 'unpaid'
end
insert into tbl_bills( pid, roombill, doctorbill, mediabill, totalbill, paidbill, remainingbill, status, edate)
values(@pid,@roombill,@doctorbill,@medicinebill,@totalbill,@paidbill,@rembill,@stat,GETDATE())
end
end
end



alter procedure usppaymentbill
(
@pid int,
@amount float,
@paytype nvarchar(50),
@check nvarchar(50),
@bank nvarchar(50)
)
as
begin
declare @checkamount float
declare @bid int
set @checkamount = (select remainingbill from tbl_bills where pid =@pid)
set @bid = (select bid from tbl_bills where pid = @pid)
if(@paytype = 'check' and @amount <= @checkamount and @amount >0 and  @checkamount !=0 )
begin
insert into tbl_payment(bid, pid, amount, payment_type, check_no, bank_name, edate) 
values(@bid,@pid,@amount,@paytype,@check,@bank,getdate())
update tbl_bills set remainingbill = remainingbill-@amount where pid = @pid
update tbl_bills set paidbill = paidbill + @amount where pid = @pid
declare @rem float
set @rem = (select remainingbill from tbl_bills where pid = @pid)
if(@rem =0)
begin
update tbl_bills set status='paid' where pid = @pid
end
end
else if(@paytype = 'cash' and @amount <= @checkamount and @amount >0 and  @checkamount !=0)
begin
declare @check1 nvarchar(50)
declare @bank1 nvarchar(50)
set @check1 = null
set @bank1 = null
insert into tbl_payment(bid, pid, amount, payment_type, check_no, bank_name, edate) 
values(@bid,@pid,@amount,@paytype,@check1,@bank1,getdate())
update tbl_bills set remainingbill = remainingbill-@amount where pid = @pid
update tbl_bills set paidbill = paidbill + @amount where pid = @pid
set @rem = (select remainingbill from tbl_bills where pid = @pid)
if(@rem =0)
begin
update tbl_bills set status='paid' where pid = @pid
end
end

end


alter procedure upsaskdischarge
(
@pid int
)
as
begin
if exists(select * from tbl_patient where pid = @pid)
begin
declare @bal float

select * from tbl_bills where pid = @pid
select p.name from tbl_bills as b join tbl_patient as p on b.pid=p.pid where p.pid=@pid

end
end

alter procedure uspdischarge
(
@pid int
)
as
begin
if exists(select * from tbl_patient where pid = @pid)
begin
declare @bal float
declare @doctname nvarchar(50)
set @doctname =(select doctor_name from tbl_patient where pid =@pid)
set @bal = (select remainingbill from tbl_bills where pid =@pid)
if(@bal = 0)
begin
update tbl_patient set status='discharged' where pid = @pid
if exists (select rid from tbl_patient where pid =@pid)
begin
declare @rid int
set @rid = (select rid from tbl_patient where pid =@pid)
update tbl_rooms set status='available' where rid =@rid
end
update tbl_bills set status='paid' where pid =@pid
update tbl_doctors set status ='available' where doctorname = @doctname
end
end
end


create procedure uspeditroom
(
@rid int,
@price float
)
as
begin
update tbl_rooms set price =@price where rid = @rid
end


alter procedure uspdeleteroom
(
@rid int
)
as
begin
delete from tbl_rooms where rid = @rid and status = 'available'
end




alter procedure uspadmitreport
(
@fromdate datetime,
@todate datetime
)
as
begin
select * from tbl_patient where convert(date,edate) between @fromdate and @todate and status ='admitted'
end

create procedure uspdischargereport
(
@fromdate datetime,
@todate datetime
)
as
begin
select * from tbl_patient where convert(date,edate) between @fromdate and @todate and status ='discharge'
end



alter procedure usppatientbill
(
@pid int
)
as
begin
if exists(select * from tbl_patient where pid = @pid)
begin
select p.pid as pid,p.name as name,p.status as patietntstatus,
p.edate as admitted_date,b.paidbill as paidbill,b.remainingbill as remainingbill
from tbl_bills as b join tbl_patient as p on p.pid=b.pid where p.pid=@pid
end
end


alter procedure usppatientpayment
(
@pid int
)
as
begin
if exists(select * from tbl_patient where pid = @pid)
begin
select p.pid as pid,p.name as name,p.status as patietntstatus,
p.edate as admitted_date,pa.bid as bid,pa.amount as amount_paid,pa.payment_type as payment_type
from tbl_payment as pa join tbl_patient as p on pa.pid=p.pid where p.pid=@pid
end
end

alter procedure usproomwisereport
(
@type nvarchar(100)
)
as
begin
select r.roomcode ,p.name as patient_name,r.roomtype as room_type from tbl_rooms as r join tbl_patient as p on r.rid=p.pid where r.roomtype=@type
end




alter procedure uspdoctorpatients
(
@name nvarchar(50)
)
as
begin
select d.doctorname,p.name as patient_name from tbl_doctors as d join tbl_patient as p on d.doctorname=p.doctor_name where d.doctorname =@name
end






create procedure uspmyprofile
(
@uid int
)
as
begin
select * from tbl_doctors where uid =@uid

end








create procedure usppatientlist
(
@uid int
)
as
begin
select d.doctorname,p.name as patient_name from tbl_doctors as d join tbl_patient as p on d.doctorname=p.doctor_name where d.uid =@uid
end




alter procedure uspmycollections
(
@uid int
)
as
begin
select p.doctor_name,sum(d.consultation_fee) as totalcollection  from tbl_doctors as d join tbl_patient as p on d.doctorname=p.doctor_name where d.uid =@uid group by p.doctor_name 
end







create procedure usproomwiserport
(
@roomtypeid int
)
as
begin
select * from tbl_roomtype as t join tbl_patient as p on t.roomtype = p.room_type
end

alter procedure uspchangepassword
(
@uid int,
@newpass nvarchar(50)
)
as
begin
update tbl_login set password =@newpass where uid=@uid

end

