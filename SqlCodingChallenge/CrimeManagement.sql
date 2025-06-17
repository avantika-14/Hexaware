IF not exists (
    SELECT NAME FROM sys.databases 
    WHERE NAME = 'crimeManagement'
)
BEGIN
    CREATE DATABASE crimeManagement
    PRINT 'database "crimeManagement" created'
END
ELSE
BEGIN
    PRINT 'database already exists'
END

USE crimeManagement



-- I creating tables

-- crime table

IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.TABLES
    WHERE TABLE_NAME = 'crime' AND TABLE_TYPE = 'BASE TABLE'
)
BEGIN
    CREATE TABLE crime (
        crimeID INT PRIMARY KEY,
        incidentType VARCHAR(255),
        incidentDate DATE,
        [location] VARCHAR(255),
        [description] TEXT,
        [status] VARCHAR(20)
    );
    PRINT 'table "crime" created.';
END
ELSE
BEGIN
    PRINT 'table already exists';
END

-- victims table

IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.TABLES
    WHERE TABLE_NAME = 'victims' AND TABLE_TYPE = 'BASE TABLE'
)
BEGIN
    CREATE TABLE victims (
    victimID INT PRIMARY KEY,
    crimeID INT,
    [name] VARCHAR(255),
    contactInfo VARCHAR(255),
    injuries VARCHAR(255),
    FOREIGN KEY (crimeID) REFERENCES crime(crimeID)
);
    PRINT 'table "victims" created.';
END
ELSE
BEGIN
    PRINT 'table already exists';
END

-- suspects table

IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.TABLES
    WHERE TABLE_NAME = 'suspects' AND TABLE_TYPE = 'BASE TABLE'
)
BEGIN
    CREATE TABLE suspects(
    suspectID INT PRIMARY KEY,
    crimeID INT,
    [name] VARCHAR(255),
    [description] TEXT,
    criminalHistory TEXT,
    FOREIGN KEY (crimeID) REFERENCES crime(crimeID)
);
    PRINT 'table "suspects" created.';
END
ELSE
BEGIN
    PRINT 'table already exists';
END



-- II insert sample data

-- Insert into crime table

INSERT INTO crime (crimeID, incidentType, incidentDate, [location], [description], [status])
VALUES
    (1, 'Robbery', '2023-09-15', '123 Main St, Cityville', 'Armed robbery at a convenience store', 'Open'),
    (2, 'Homicide', '2023-09-20', '456 Elm St, Townsville', 'Investigation into a murder case', 'Under Investigation'),
    (3, 'Theft', '2023-09-10', '789 Oak St, Villagetown', 'Shoplifting incident at a mall', 'Closed');

-- Insert into victims table

INSERT INTO victims (victimID, crimeID, [name], contactInfo, injuries)
VALUES
    (1, 1, 'John Doe', 'johndoe@example.com', 'Minor injuries'),
    (2, 2, 'Jane Smith', 'janesmith@example.com', 'Deceased'),
    (3, 3, 'Alice Johnson', 'alicejohnson@example.com', 'None');

-- Insert into suspects table

INSERT INTO suspects (suspectID, crimeID, [name], [description], criminalHistory)
VALUES
    (1, 1, 'Robber 1', 'Armed and masked robber', 'Previous robbery convictions'),
    (2, 2, 'Unknown', 'Investigation ongoing', NULL),
    (3, 3, 'Suspect 1', 'Shoplifting suspect', 'Prior shoplifting arrests');



-- III solving queries

-- 1 select all open incidents
SELECT * FROM crime WHERE [status] = 'Open';


-- 2 find the total number of incidents
SELECT count(*) TotalIncidents FROM crime;


-- 3 list all unique incident types
SELECT DISTINCT incidentType FROM crime;

-- 4 retreive incidents between 2023-09-01 amd 2023-09-10
SELECT * FROM crime        
    WHERE incidentDate BETWEEN '2023-09-01' AND '2023-09-10';


-- 5 list person involved in incidents in descending order of age
ALTER TABLE victims ADD age INT;
update victims
SET age = CASE victimID
    WHEN 1 THEN 34
    WHEN 2 THEN 28
    WHEN 3 THEN 22
END
WHERE victimID IN (1, 2, 3);

ALTER TABLE suspects ADD age INT;
update suspects
SET age = CASE suspectID
    WHEN 1 THEN 40
    WHEN 2 THEN 35
    WHEN 3 THEN 30
END
WHERE suspectID IN (1, 2, 3);

select name,age,'victim' AS involvement FROM victims
UNION
select name,age,'suspect' AS involvement FROM suspects;


-- 6 find average age of person involved
SELECT avg(age) AS AverageAge FROM(
    SELECT age FROM victims
    UNION all
    SELECT age FROM suspects
) AS people;


-- adding additional values in the table

-- crime
INSERT INTO crime (crimeID, incidentType, incidentDate, [location], [description], [status])
VALUES
        (4, 'Assault', '2023-09-05', '321 Pine St, Newtown', 'Bar fight involving multiple suspects', 'Open'),
    (5, 'Robbery', '2023-09-01', '654 Cedar St, Cityville', 'ATM robbery', 'Open'),
    (6, 'Cybercrime', '2023-09-18', '987 Binary Rd, Techville', 'Phishing attack reported', 'Open'),
    (7, 'Homicide', '2023-09-22', '999 Blood Rd, Murderville', 'New homicide case', 'Open'),
    (8, 'Robbery', '2023-09-23', '101 Steal St, Grabtown', 'Pickpocket case', 'Open'),
    (9, 'Robbery', '2023-09-24', '202 Swipe Ln, Lootcity', 'Bank robbery attempt', 'Open'),
    (10, 'Robbery', '2023-09-25', '303 Shadow Rd, Nightcity', 'Jewelry store robbery', 'Open');

--victims
INSERT INTO victims (victimID, crimeID, [name], contactInfo, injuries, age)
VALUES
    (4, 4, 'Michael Doe', 'michael@example.com', 'Broken nose', 30),       
    (5, 5, 'Robber 1', 'robber1victim@example.com', 'None', 36),           
    (6, 4, 'Sarah Lee', 'sarah@example.com', 'Bruises', 35),
    (7, 6, 'Cyber Victim', 'cyber@example.com', 'Identity theft', 29);             

-- suspects
INSERT INTO suspects (suspectID, crimeID, [name], [description], criminalHistory, age)
VALUES
    (4, 4, 'Michael Doe', 'Aggressor in bar fight', 'Past assault charges', 32),  
    (5, 5, 'Robber 2', 'ATM theft specialist', 'Prior robberies', 27),
    (6, 4, 'John Doe', 'Repeat offender', 'None', 50),
    (7, 5, 'Michael Doe', 'Repeat suspect in ATM robbery', 'Past assault charges', 32),                    
    (8, 10, 'John Doe', 'Repeat robbery suspect', 'Involved in multiple robberies', 50);


-- 7 list incident types and their counts, only for open cases
SELECT incidentType, count(*) AS OpenCases
    FROM crime
    WHERE [status] = 'Open'
    GROUP BY incidentType;

    
-- 8 find person name containing doe
SELECT [name],'victim' AS involvement FROM victims WHERE [name] LIKE '%Doe%'
UNION
SELECT [name],'suspects' AS involvement FROM suspects WHERE [name] like '%Doe%';


-- 9 retreive names of persons involved in open and closed cases
SELECT DISTINCT v.name FROM victims v
    join crime c ON v.crimeID = c.crimeID
    WHERE c.status in ('Open','Closed')
UNION
SELECT DISTINCT s.name FROM suspects s
    join crime c ON s.crimeID = c.crimeID
    WHERE c.status in ('Open','Closed');


-- 10 liat person types where persons aged 30 or 35 are involved
SELECT DISTINCT c.IncidentType
    FROM crime c
    left join victims v ON c.crimeID = v.crimeID
    left join suspects s ON c.crimeID = s.crimeID
    WHERE v.age in (30,35) or s.age in (30,35); 


-- 11 find the persons involved in incidents of the same type as 'robbery'
SELECT DISTINCT v.name FROM victims v
    join crime c ON v.crimeID = c.crimeID
    WHERE c.incidentType = 'Robbery'
UNION 
SELECT DISTINCT s.name FROM suspects s
    join crime c ON s.crimeID = c.crimeID
    WHERE c.incidentType = 'Robbery';


-- 12 incident types with more than 1 open cases
SELECT incidentType, count(*) AS caseCount 
    FROM crime
    WHERE status = 'Open'
    GROUP BY incidentType
    HAVING count(*) > 1;


-- 13 incidents where suspects names appear as victims in other incidents
SELECT DISTINCT 
    c.crimeID, c.incidentType, c.incidentDate, c.location, c.status
    FROM crime c
    join suspects s ON c.crimeID = s.crimeID
    WHERE s.name IN (
        SELECT name FROM victims
    );


-- 14 retreive all details with victims and suspects details
SELECT c.*,v.name as victimName, s.name as suspectName
    FROM crime c
    left join victims v ON c.crimeID = v.crimeID
    left join suspects s on c.crimeID = v.crimeID;


-- 15 incidents where the suspect is older than victims
SELECT DISTINCT
    c.crimeID, c.incidentType, c.incidentDate, c.location, c.status
    FROM crime c
    join victims v ON c.crimeID = v.crimeID
    join suspects s ON c.crimeID = s.crimeID
    WHERE s.age > v.age;


-- 16 suspects involved in multiple incidents
SELECT [name], count(DISTINCT crimeID) AS incidentCount
    FROM suspects
    GROUP BY [name]
    HAVING count(DISTINCT crimeID) > 1;


-- 17 incidents with no suspects involved
select c.* 
    FROM crime c
    left join suspects s ON c.crimeID = s.crimeID
    WHERE s.crimeID is null;


-- 18 cases with atleast one homicide and all other robbery
update suspects
SET name = 'Unknown',[description] = 'same suspect from homicide now in robbery', criminalHistory = 'suspected robbery earlier'
WHERE suspectID = 8;

SELECT s.suspectID, s.name, c.incidentType, s.description, s.criminalHistory
    FROM suspects s
    join crime c ON s.crimeID = c.crimeID
    WHERE s.name in (
        SELECT s.name
        FROM suspects s
        join crime c ON s.crimeID = c.crimeID
        WHERE c.incidentType in ('Homicide', 'Robbery')
        GROUP BY s.name
        HAVING 
            count(DISTINCT CASE WHEN c.incidentType = 'Homicide' THEN 1 END) >= 1 AND
            count(DISTINCT CASE WHEN c.incidentType = 'Robbery' THEN 1 END) >= 1
    );


-- 19 incidents and suspects, show 'no suspects' if none
SELECT c.crimeID, c.incidentType, ISNULL(s.name, 'No Suspects') 
    AS suspectName
    FROM crime c
    left join suspects s ON c.crimeID = s.crimeID;


-- 20 suspects in robbery or assault incidents
SELECT DISTINCT 
    s.suspectID, s.crimeID, s.name
    FROM suspects s
    join crime c ON s.crimeID = c.crimeID
    WHERE c.incidentType in ('Robbery', 'Assault');



