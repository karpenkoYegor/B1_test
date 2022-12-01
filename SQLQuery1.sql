/****** Script for SelectTopNRows command from SSMS  ******/
SELECT  TOP(1) PERCENTILE_DISC(0.5) 
	WITHIN GROUP (ORDER BY FracNumber) 
	OVER()
FROM [B1Db].[dbo].[MyStrings]
SELECT SUM(EvenNumber) as sum FROM [B1Db].[dbo].[MyStrings]