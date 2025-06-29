create database virtualArtGallery;

use virtualArtGallery;

-- 1. creating tables

create table artworks (
    artworkID int primary key identity(101,1),
    title nvarchar(100),
    description nvarchar(max),
    creationDate date,
    [medium] nvarchar(100),
    imageURL nvarchar(255)
);

create table artists (
	artistID int primary key identity (501,1),
	[name] nvarchar (100),
	biography nvarchar (max),
	birthDate date,
	nationality nvarchar (50),
	website nvarchar (max),
	contactInformation nvarchar (250)
);

create table [users] (
	userID int primary key identity,
	username nvarchar (50),
	[password] nvarchar (100),
	email nvarchar (100),
	firstName nvarchar (50),
	lastName nvarchar (50),
	dateOfBirth date,
	profilePicture varbinary(max)
);

create table gallery (
	galleryID int primary key identity (1201,1),
	[name] varchar (100),
	[description] nvarchar (max),
	[location] nvarchar (150),
	curator int foreign key references artists(artistID),
	openingHours nvarchar (15)
);

-- 2. relationships

-- artwork - artist - many to one

alter table artworks
	add artistID int foreign key 
	references artists (artistID)

-- user favorite artwork - many to many

create table user_favorite_artworks (
	userID int foreign key references [users](userID),
	artworkID int foreign key references artworks(artworkID),
	primary key (userID, artworkID)
);

-- artist - gallery - one to many 
-- curator int foreign key references artists (artistId)

-- artwork - gallery - many to many

create table artwork_gallery (
	artworkID int foreign key references artworks (artworkID),
	galleryID int foreign key references gallery (galleryID),
	primary key (artworkID, galleryID)
);

-- 3. inserting values into the tables

insert into artists ([name], biography, birthDate, nationality, website, contactInformation)
	values
	('Leonardo da Vinci', 'Renaissance painter and inventor.', '1452-04-15', 'Italian', 'https://da-vinci.art', 'contact@davinci.art'),
	('Vincent van Gogh', 'Post-Impressionist Dutch painter.', '1853-03-30', 'Dutch', 'https://vangogh.art', 'info@vangogh.art'),
	('Frida Kahlo', 'Mexican painter known for self-portraits.', '1907-07-06', 'Mexican', 'https://frida.art', 'frida@art.com'),
	('Pablo Picasso', 'Spanish painter, sculptor, printmaker.', '1881-10-25', 'Spanish', 'https://picasso.art', 'picasso@artworld.com'),
	('Claude Monet', 'Founder of French Impressionism.', '1840-11-14', 'French', 'https://monet.art', 'contact@monet.fr'),
	('Andy Warhol', 'Pop artist and cultural icon.', '1928-08-06', 'American', 'https://warhol.org', 'info@warhol.org'),
	('Georgia O’Keeffe', 'Modernist artist of American Southwest.', '1887-11-15', 'American', 'https://okeeffe.org', 'hello@okeeffe.org'),
	('Salvador Dalí', 'Spanish surrealist painter.', '1904-05-11', 'Spanish', 'https://dali.art', 'dali@dreams.com'),
	('Rembrandt', 'Dutch Golden Age painter.', '1606-07-15', 'Dutch', 'https://rembrandt.nl', 'info@rembrandt.nl'),
	('Michelangelo', 'Sculptor and painter of the High Renaissance.', '1475-03-06', 'Italian', 'https://michelangelo.art', 'contact@michelangelo.it'),
	('Banksy', 'Anonymous England-based street artist.', '1974-01-01', 'British', 'https://banksy.co.uk', 'anonymous@banksy.art'),
	('Yayoi Kusama', 'Japanese contemporary artist.', '1929-03-22', 'Japanese', 'https://kusama.jp', 'kusama@dots.com'),
	('Jackson Pollock', 'Abstract expressionist.', '1912-01-28', 'American', 'https://pollock.art', 'pollock@drip.com'),
	('Henri Matisse', 'French visual artist.', '1869-12-31', 'French', 'https://matisse.art', 'info@matisse.fr'),
	('Roy Lichtenstein', 'Pop art movement leader.', '1923-10-27', 'American', 'https://lichtenstein.art', 'roy@popart.com');

insert into artworks (title, [description], creationDate, [medium], imageURL, artistID) 
	values
	('Mona Lisa', 'Portrait of a woman by Leonardo da Vinci.', '1503-06-01', 'Oil on wood', 'https://images.com/monalisa.jpg', 501),
	('Starry Night', 'Night sky by van Gogh.', '1889-06-01', 'Oil on canvas', 'https://images.com/starrynight.jpg', 502),
	('The Two Fridas', 'Self-portrait of Frida Kahlo.', '1939-01-01', 'Oil on canvas', 'https://images.com/fridas.jpg', 503),
	('Guernica', 'Anti-war mural by Picasso.', '1937-06-01', 'Oil on canvas', 'https://images.com/guernica.jpg', 504),
	('Water Lilies', 'Peaceful pond scenes.', '1916-01-01', 'Oil on canvas', 'https://images.com/lilies.jpg', 505),
	('Campbell’s Soup Cans', 'Pop art work by Warhol.', '1962-01-01', 'Synthetic polymer paint', 'https://images.com/soup.jpg', 506),
	('Red Canna', 'Close-up of a flower.', '1924-01-01', 'Oil on canvas', 'https://images.com/redcanna.jpg', 507),
	('Persistence of Memory', 'Melting clocks.', '1931-01-01', 'Oil on canvas', 'https://images.com/memory.jpg', 508),
	('The Night Watch', 'Militia company scene.', '1642-01-01', 'Oil on canvas', 'https://images.com/nightwatch.jpg', 509),
	('The Creation of Adam', 'Iconic fresco by Michelangelo.', '1512-01-01', 'Fresco', 'https://images.com/adam.jpg', 510),
	('Girl with Balloon', 'Banksy’s famous stencil.', '2002-01-01', 'Spray paint', 'https://images.com/balloon.jpg', 511),
	('Infinity Mirror Room', 'Immersive installation.', '2013-01-01', 'Mixed media', 'https://images.com/mirrors.jpg', 512),
	('Number 5', 'Drip painting.', '1948-01-01', 'Oil on fiberboard', 'https://images.com/number5.jpg', 513),
	('The Dance', 'Bright colored dancers.', '1910-01-01', 'Oil on canvas', 'https://images.com/dance.jpg', 514),
	('Whaam!', 'Comic-style pop art.', '1963-01-01', 'Acrylic and oil', 'https://images.com/whaam.jpg', 515);

insert into [users] (username, [password], email, firstName, lastName, dateOfBirth, profilePicture) 
	values
	('alice123', 'hashed_pwd1', 'alice@example.com', 'Alice', 'Brown', '1995-04-12', null),
	('bob_rock', 'hashed_pwd2', 'bob@example.com', 'Bob', 'Rockwell', '1993-11-03', null),
	('charlie_x', 'hashed_pwd3', 'charlie@example.com', 'Charlie', 'Xavier', '1990-09-21', null),
	('daniel4', 'hashed_pwd4', 'daniel@example.com', 'Daniel', 'Lee', '1992-01-16', null),
	('emma.arts', 'hashed_pwd5', 'emma@example.com', 'Emma', 'Watts', '1994-07-08', null),
	('fiona99', 'hashed_pwd6', 'fiona@example.com', 'Fiona', 'Scott', '1991-02-02', null),
	('george_z', 'hashed_pwd7', 'george@example.com', 'George', 'Zimmer', '1989-06-30', null),
	('harry_art', 'hashed_pwd8', 'harry@example.com', 'Harry', 'Styles', '1993-10-25', null),
	('ian_artist', 'hashed_pwd9', 'ian@example.com', 'Ian', 'Painter', '1996-05-14', null),
	('jane_d', 'hashed_pwd10', 'jane@example.com', 'Jane', 'Doe', '1992-08-19', null),
	('kevinx', 'hashed_pwd11', 'kevin@example.com', 'Kevin', 'Black', '1995-12-04', null),
	('lisa_m', 'hashed_pwd12', 'lisa@example.com', 'Lisa', 'Moon', '1990-03-29', null),
	('michael_art', 'hashed_pwd13', 'michael@example.com', 'Michael', 'White', '1988-07-17', null),
	('nina22', 'hashed_pwd14', 'nina@example.com', 'Nina', 'Patel', '1994-01-01', null),
	('oliver_art', 'hashed_pwd15', 'oliver@example.com', 'Oliver', 'Grant', '1997-10-10', null);

insert into gallery ([name], [description], [location], curator, openingHours) values
('Modern Masters', 'Featuring modern era artwork.', 'New York, USA', 501, '10:00-18:00'),
('Impression Lights', 'Impressionist and light-themed works.', 'Paris, France', 502, '09:00-17:00'),
('Self Reflections', 'Focus on personal and emotional art.', 'Mexico City, Mexico', 503, '10:30-19:00'),
('War & Peace', 'Gallery depicting conflict and harmony.', 'Madrid, Spain', 504, '11:00-18:00'),
('Garden & Water', 'Nature themed artwork.', 'Giverny, France', 505, '09:00-16:00'),
('Pop Culture Hub', 'Icons of pop art.', 'Pittsburgh, USA', 506, '12:00-20:00'),
('Desert Dreams', 'Art inspired by landscapes.', 'Santa Fe, USA', 507, '10:00-17:00'),
('Surreal Visions', 'Surrealist masterworks.', 'Figueres, Spain', 508, '11:00-19:00'),
('Dutch Gold', 'Art from the Netherlands.', 'Amsterdam, Netherlands', 509, '09:30-17:00'),
('Renaissance Hall', 'Italian classical masterpieces.', 'Florence, Italy', 510, '08:00-18:00'),
('The Anonymous Wall', 'Urban and street art.', 'London, UK', 511, '10:00-21:00'),
('Mirror World', 'Immersive installations.', 'Tokyo, Japan', 512, '13:00-20:00'),
('Abstract Flow', 'Abstract expressionism focus.', 'New York, USA', 513, '10:00-17:00'),
('Color & Joy', 'Color-rich pieces.', 'Nice, France', 514, '10:00-18:30'),
('Comic Canvas', 'Cartoon-inspired expressions.', 'Chicago, USA', 515, '11:00-19:00');

insert into user_favorite_artworks (userID, artworkID) values
(1, 101), (1, 102),
(2, 103), (2, 104),
(3, 105), (3, 101),
(4, 106), (4, 107),
(5, 108), (5, 109),
(6, 110), (6, 102),
(7, 111), (7, 112),
(8, 113), (8, 103),
(9, 114), (9, 105),
(10, 115), (10, 104),
(11, 101),
(12, 102),
(13, 103),
(14, 104),
(15, 105);

insert into artwork_gallery (artworkID, galleryID) values
(101, 1201), (101, 1202),
(102, 1203), (102, 1201),
(103, 1204),
(104, 1205), (104, 1206),
(105, 1207),
(106, 1208),
(107, 1209), (107, 1210),
(108, 1211),
(109, 1212),
(110, 1213), (110, 1214),
(111, 1215),
(112, 1202),
(113, 1203),
(114, 1204),
(115, 1205);
