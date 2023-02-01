INSERT into AspNetRoles (Id,Name,NormalizedName,ConcurrencyStamp) VALUES 
('b4029371-be9c-45a1-94a4-ccac2dff88db', 'Admin', 'ADMIN', 'cc7ff09f-6101-4c22-928b-c3040a1f36b5'),
('efcb9598-9ac4-47de-9a9a-583c5192facb', 'Operador', 'OPERADOR', '43868724-7058-4f92-8b60-62d44990706c'),
('b73a8dfc-4c15-474f-a30d-7df6f48179fa', 'Coordenador', 'COORDENADOR', 'ee5f4623-c64d-4fff-a936-28c13d606bd0'),
('091e4c3d-0aa4-48a6-b201-6e69c9577e51', 'Design', 'DESIGN', 'a6939c53-1a5c-4309-be1e-1492a46347b6'),
('a296fc56-ef20-4060-a65a-3e1385ff49de', 'Editor', 'EDITOR', '1acec6d4-88e7-4cfb-b1c3-8ab369b10c6b'),
('ec27cb0e-579c-45f3-94e8-25b92dc4a2d9', 'Gerenciamento','GERENCIAMENTO', 'f31cf581-ee6c-4fba-b1a6-51ff65fd0825'),
('84af70b4-b127-4292-aed9-d6e83125dba2', 'Fotografia','FOTOGRAFIA', 'e21b3df9-1603-47b7-8c64-c57a6d1e5de4');

INSERT AspNetUsers (Id, AccessFailedCount, ConcurrencyStamp, 
Email, EmailConfirmed, LockoutEnabled, LockoutEnd, NormalizedEmail, 
NormalizedUserName, PasswordHash, PhoneNumber, PhoneNumberConfirmed, 
SecurityStamp, TwoFactorEnabled, UserName, IsDeleted, NickName) VALUES 
('45005005-e383-4d12-9e4f-46370c72908d', 0, 
'5e8ac58f-d771-423a-8f89-267882dff51d', 'admin@email.com', 0, 1, NULL, 
'ADMIN@EMAIL.COM', 'ADMIN@EMAIL.COM', 
N'AQAAAAEAACcQAAAAEMMhA5MGxx0SVY17VLgMi98FmTt1p0LT/TFev7sY7dcY8MLWnVAdhsP3uxqPt75/HA==', 
null, 0, N'7AUTLG5PVJS2ROMBFXIR2SPMRWNBC2XF', 0, 'admin@email.com', 0, 'Admin');

INSERT into TypeGenders (Id,Name) VALUES 
('db0758b1-def7-4237-947f-7cc43c7c48a2', 'Masculino'),
('caa850c5-3bd2-454d-8850-4564dd4575d0', 'Feminino'),
('31c29543-76a9-4fdc-8953-b5bab8700455', 'Outro (Qual?)'),
('f7c491b6-fd5d-4a56-a0e2-8f024b51a9b5', 'Prefiro não dizer');

INSERT Genders (Id, TypeGenderId) values 
('253c7f6d-edcc-4b07-b44b-27e78056a821', 'db0758b1-def7-4237-947f-7cc43c7c48a2');

INSERT PhysicalPersons (Id, CPF, Name, UserId, GenderId) values 
('253c7f6d-edcc-4b07-b44b-27e78056a821', '', 'Admin', '45005005-e383-4d12-9e4f-46370c72908d', '253c7f6d-edcc-4b07-b44b-27e78056a821');

INSERT BackOfficeUsers (Id, UserId) values 
('740e23ba-b46c-4718-a3c8-b342662a6aab', '45005005-e383-4d12-9e4f-46370c72908d');

INSERT AspNetUserRoles (UserId, RoleId) VALUES
('45005005-e383-4d12-9e4f-46370c72908d', 'b4029371-be9c-45a1-94a4-ccac2dff88db');

