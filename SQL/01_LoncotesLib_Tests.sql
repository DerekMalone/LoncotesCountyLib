SELECT * FROM "Patrons"
SELECT * FROM "Checkouts"
SELECT * FROM "Materials"
SELECT * FROM "MaterialTypes"

SELECT 
	p."FirstName",
	p."LastName",
	c."Id" AS Checkout_Id,
	c."CheckoutDate",
	C."ReturnDate",
	m."MaterialName",
	mt."Name"
FROM "Patrons" p
LEFT JOIN "Checkouts" c ON p."Id" = c."PatronId"
LEFT JOIN "Materials" m ON m."Id" = c."MaterialId"
LEFT JOIN "MaterialTypes" mt ON mt."Id" = m."MaterialTypeId"

INSERT INTO "Checkouts" ("Id", "MaterialId", "PatronId", "CheckoutDate", "ReturnDate") 
VALUES (1, 1, 1, '2025, 04, 29', '2025, 05, 05')

INSERT INTO "Checkouts" ("Id", "MaterialId", "PatronId", "CheckoutDate", "ReturnDate") 
VALUES (2, 2, 2, '2025, 04, 29', '2025, 05, 05')

INSERT INTO "Checkouts" ("Id", "MaterialId", "PatronId", "CheckoutDate", "ReturnDate") 
VALUES (3, 2, 1, '2025, 04, 29', '2025, 05, 05')

UPDATE "Patrons"
SET "IsActive" = true
WHERE "Id" = 2;

