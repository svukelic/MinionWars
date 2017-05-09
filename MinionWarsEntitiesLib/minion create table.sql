CREATE TABLE Minion(
	id int primary key identity(1,1),
	mtype_id int not null,
	somatotype varchar(3) not null,
	melee_ability int not null,
	ranged_ability int not null,
	passive int not null,
	speed float not null,
	strength int not null,
	dexterity int not null,
	vitality int not null,
	line int not null,
	battlegroup_id int not null,
	group_count int not null,
	CONSTRAINT fk_minion_mtype FOREIGN KEY (mtype_id) REFERENCES MinionType(id),
	CONSTRAINT fk_minion_bg FOREIGN KEY (id) REFERENCES Battlegroup(id)
);