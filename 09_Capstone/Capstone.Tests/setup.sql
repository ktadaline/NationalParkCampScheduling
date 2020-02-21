-- Delete all of the data
DELETE FROM reservation;
DELETE FROM site;
DELETE FROM campground;
DELETE FROM park;

-- Insert a fake department
INSERT INTO park
(name, location, establish_date, area, visitors, description )
VALUES
('Crystal', 'Michigan', '2001-01-01', '45', '500', 'Fantastico'),
('Math park', 'Ohio', '2004-10-01', '65000', '3', 'Gut'), 
('Tech Elevator Park', 'Codeville', '2015-01-01', '6', 'The best');


DECLARE @newdpark_id int = (SELECT @@IDENTITY)

-- Insert a fake employee
INSERT INTO campground
(park_id, name, open_from_mm, open_to_mm, daily_fee)
VALUES
((SELECT park.park_id FROM park WHERE park_name = 'Crystal'), 'Camp Blue', 4, 11, 10.00),
((SELECT park.park_id FROM park WHERE park_name = 'Math park'), 'Camp Red', 5, 10, 100.00),
((SELECT park.park_id FROM park WHERE park_name = 'Tech Elevator'), 'Camp Code', 1, 4, 15000.00)

DECLARE @newcampground_id int = (SELECT @@IDENTITY)


--UPDATED Department ID so that its not hard coded

--Insert a fake site

INSERT INTO site
(campground_id, site_number, max_occupancy, accessible, max_rv_length, utilities)
Values 
((SELECT campground.campground_id FROM campground WHERE campground.name = 'Camp Blue'), 1, 4, 0, 20, 0),
((SELECT campground.campground_id FROM campground WHERE campground.name = 'Camp Red'), 2, 6, 1, 25, 1),
((SELECT campground.campground_id FROM campground WHERE campground.name = 'Camp Code'), 3, 20, 0, 20, 0)


DECLARE @newsite_id int = (SELECT @@IDENTITY)

--Insert a fake project employee
INSERT INTO reservation
(site_id, name, from_date, to_date, create_date)
Values

((SELECT site_id FROM site WHERE site.site_number = 1), 'Bobby Jones', '2001-10-10','2001-10-12', CURRENT_TIMESTAMP),
((SELECT site_id FROM site WHERE site.site_number = 2), 'Mike Morel', '2020-02-20','2020-02-25', CURRENT_TIMESTAMP),
((SELECT site_id FROM site WHERE site.site_number = 3), 'Paris Hilton', '2020-03-04','2020-06-09', CURRENT_TIMESTAMP)

DECLARE @newreservation_id int = (SELECT @@IDENTITY)

--UPDATED Project ID so that its not hard coded

SELECT @newpark_id as newpark_id, @newcampground_id as newcampground_id, @newsite_id as newsite_id, @newreservation_id as newreservation_id



