CREATE TABLE HiveNode(
	id int primary key identity(1,1),
	location_id int not null,
	minion_id int not null,
	CONSTRAINT fk_hnode_loc FOREIGN KEY (location_id) REFERENCES Location(id),
	CONSTRAINT fk_hnode_minion FOREIGN KEY (minion_id) REFERENCES Minion(id)
);