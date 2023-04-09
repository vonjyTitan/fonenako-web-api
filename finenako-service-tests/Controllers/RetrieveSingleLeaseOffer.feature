Feature: Retrieve single lease offer
	In order to have a full details of a lease offer
	I want to call the GET api with my Offer ID

Background:
	Given The following list of city is present in the system
		|CityId|Name  |
		|1	   | City1|
	Given The following list of area is present in the system
		|AreaId|CityId|Name  |
		|1	   |1     | Area1|
		|2	   |1     | Area2|
		|3	   |1     | Area3|

Scenario: Retrieve single lease offer with invalid id
	Given Whatever data I have in the system
	When I make a GET request on lease-offers endpoint with offer id : '-1'
	Then The response Status code should be '400'

Scenario: Retrieve single lease offer with a not existing lease offer id
	Given The following list of lease offer is present in the system
		|LeaseOfferID |AreaId|Title          |Surface |Rooms|MonthlyRent|Creationdate|ConcatenedPhotos			    |
		|1            |1	 |Offer number 1 |18      |3    |800		 |2023-09-25  |image1.jpg;image2.jpg|
	When I make a GET request on lease-offers endpoint with offer id : '2'
	Then The response Status code should be '404'

Scenario: Retrieve single lease offer with an existing lease offer id
	Given The following list of lease offer is present in the system
		|LeaseOfferID |AreaId	|Title          |Surface |Rooms|MonthlyRent|Description  |Creationdate|ConcatenedPhotos			    |
		|1            |1		|Offer number 1 |18      |3    |800		 |Description 1|2023-09-25  |image1.jpg;image2.jpg|
		|2            |1		|Offer number 2 |40      |1    |900		 |Description 2|2023-10-25  |image3.jpg;|
	When I make a GET request on lease-offers endpoint with offer id : '2'
	Then The response Status code should be '200'
	And The body content should be like :
		|LeaseOfferID |Area.City.Id|Area.City.Name|Area.Id|Area.Name|Title          |Surface |Rooms|MonthlyRent|Description  |Creationdate|PhotoUris																	  |
		|2            |1		   |City1		  |1	  |Area1	|Offer number 2 |40      |1    |900		 |Description 2|2023-10-25  |http://localhost:7182/Photos/image3.jpg|