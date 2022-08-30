use doctor_management
select * from tbl_roles
select * from tbl_roomtype
select * from tbl_speciality
select * from tbl_login
select * from tbl_doctors
select * from tbl_rooms
select * from tbl_patient
select * from tbl_bills
select * from tbl_payment

delete from tbl_patient where pid =5
delete from tbl_bills where bid =4

update tbl_doctors set status='available' where did =1
update tbl_doctors set status='available' where did =2
update tbl_doctors set status='available' where did =3


truncate table tbl_payment

delete from tbl_bills 
dbcc checkident('[doctor_management].[dbo].[tbl_bills]',reseed,0)

delete from tbl_patient
dbcc checkident('[doctor_management].tbl_patient',reseed,0)

delete from tbl_rooms
dbcc checkident('[doctor_management].tbl_rooms',reseed,0)


delete from tbl_doctors
dbcc checkident('[doctor_management].tbl_doctors',reseed,0)

delete from tbl_login
dbcc checkident('[doctor_management].tbl_login',reseed,0)





update tbl_rooms set status='available' where rid =2
update tbl_rooms set status='available' where rid =3
update tbl_rooms set status='available' where rid =4
update tbl_patient set rid =1 where pid =2
update tbl_doctors set status='available' where did =2
update tbl_rooms set roomcode='ICU-2' WHERE rid =4


 


