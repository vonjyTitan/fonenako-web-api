Feature: Find many lease offers
	In order to obtain a leas offer list

Background:
	Given The following list of localisations is present in the system
		|LocalisationId|Type  |HierarchyId|Name  |
		|1			   |CIT   |			  |City1 |
		|2			   |ARE   | 1		  |Area1 |
		|3			   |ARE   | 1		  |Area2 |

Scenario: Retrieve lease offer list without any parameter
	Given The following list of lease offer is present in the system
		|LeaseOfferID |LocalisationId	|Title          |Surface |Rooms|MonthlyRent|CreationDate|ConcatenedPhotos			    |Description|
		|1            |2		|Offer number 1 |65      |3    |750		 |2023-10-25  |image1.jpg;image2.jpg|desc 1|
		|2            |2		|Offer number 2 |18      |1    |950		 |2023-09-25  |image3.jpg;|desc 2|
	When I make a GET request on lease-offers endpoint without any argument
	Then The response Status code should be '200'
	And The pageable infos should be like : {CurrentPage : '1', TotalPage : '1', PageSize : '10000', totalFound : '2'}
	And The pageable content items should be like :
		|LeaseOfferID |Loc.Id|Loc.Type|Loc.Name|Loc.Hier.Id|Loc.Hier.Type|Loc.Hier.Name|Title          |Surface |Rooms|MonthlyRent|CreationDate|PhotoUris																	  |Description|
		|2            |2	 |ARE     |Area1   |1		   |CIT			 |City1		   |Offer number 2 |18      |1    |950		 |2023-09-25  |http://localhost:7182/Photos/image3.jpg|desc 2|
		|1            |2	 |ARE     |Area1   |1	       |CIT			 |City1		   |Offer number 1 |65      |3    |750		 |2023-10-25  |http://localhost:7182/Photos/image1.jpg;http://localhost:7182/Photos/image2.jpg|desc 1|

Scenario: Retrieve lease offer list order by leaseOfferId asc
	Given The following list of lease offer is present in the system
		|LeaseOfferID |LocalisationId	|Title          |Surface |Rooms|MonthlyRent|CreationDate|ConcatenedPhotos			    |Description|
		|1            |2				|Offer number 1 |65      |3    |950		 |2023-09-25  |image1.jpg;image2.jpg|desc 1|
		|2            |2				|Offer number 2 |18      |1    |750		 |2023-10-25  |image3.jpg;|desc 2|
	When I make a GET request on lease-offers endpoint with arguments 'orderBy=leaseOfferId&order=Asc'
	Then The response Status code should be '200'
	And The pageable infos should be like : {CurrentPage : '1', TotalPage : '1', PageSize : '10000', totalFound : '2'}
	And The pageable content items should be like :
		|LeaseOfferID |Loc.Id|Loc.Type|Loc.Name|Loc.Hier.Id|Loc.Hier.Type|Loc.Hier.Name|Title          |Surface |Rooms|MonthlyRent|CreationDate|PhotoUris|Description|
		|1            |2	 |ARE     |Area1   |1		   |CIT			 |City1		   |Offer number 1 |65      |3    |950		 |2023-09-25  |http://localhost:7182/Photos/image1.jpg;http://localhost:7182/Photos/image2.jpg|desc 1|
		|2            |2	 |ARE     |Area1   |1	       |CIT			 |City1		   |Offer number 2 |18      |1    |750		 |2023-10-25  |http://localhost:7182/Photos/image3.jpg|desc 2|

Scenario: Retrieve lease offer list order by leaseOfferId desc
	Given The following list of lease offer is present in the system
		|LeaseOfferID |LocalisationId	|Title          |Surface |Rooms|MonthlyRent|CreationDate|ConcatenedPhotos			    |Description|
		|1            |2				|Offer number 1 |65      |3    |750		 |2023-09-25  |image1.jpg;image2.jpg|desc 1|
		|2            |2				|Offer number 2 |18      |1    |950		 |2023-10-25  |image3.jpg;|desc 2|
	When I make a GET request on lease-offers endpoint with arguments 'orderBy=leaseOfferId&order=Desc'
	Then The response Status code should be '200'
	And The pageable infos should be like : {CurrentPage : '1', TotalPage : '1', PageSize : '10000', totalFound : '2'}
	And The pageable content items should be like :
		|LeaseOfferID |Loc.Id|Loc.Type|Loc.Name|Loc.Hier.Id|Loc.Hier.Type|Loc.Hier.Name|Title          |Surface |Rooms|MonthlyRent|CreationDate|PhotoUris|Description|
		|2            |2	 |ARE     |Area1   |1		   |CIT			 |City1		   |Offer number 2 |18      |1    |950		 |2023-10-25  |http://localhost:7182/Photos/image3.jpg|desc 2|
		|1            |2	 |ARE     |Area1   |1	       |CIT			 |City1		   |Offer number 1 |65      |3    |750		 |2023-09-25  |http://localhost:7182/Photos/image1.jpg;http://localhost:7182/Photos/image2.jpg|desc 1|

Scenario: Retrieve lease offer list order by monthlyRent asc
	Given The following list of lease offer is present in the system
		|LeaseOfferID |LocalisationId	|Title          |Surface |Rooms|MonthlyRent|CreationDate|ConcatenedPhotos			    |Description|
		|1            |2				|Offer number 1 |65      |3    |950		 |2023-09-25  |image1.jpg;image2.jpg|desc 1|
		|2            |2				|Offer number 2 |18      |1    |750		 |2023-10-25  |image3.jpg;|desc 2|
	When I make a GET request on lease-offers endpoint with arguments 'orderBy=monthlyRent&order=Asc'
	Then The response Status code should be '200'
	And The pageable infos should be like : {CurrentPage : '1', TotalPage : '1', PageSize : '10000', totalFound : '2'}
	And The pageable content items should be like :
		|LeaseOfferID |Loc.Id|Loc.Type|Loc.Name|Loc.Hier.Id|Loc.Hier.Type|Loc.Hier.Name|Title          |Surface |Rooms|MonthlyRent|CreationDate|PhotoUris|Description|
		|2            |2	 |ARE     |Area1   |1		   |CIT			 |City1		   |Offer number 2 |18      |1    |750		 |2023-10-25  |http://localhost:7182/Photos/image3.jpg|desc 2|
		|1            |2	 |ARE     |Area1   |1	       |CIT			 |City1		   |Offer number 1 |65      |3    |950		 |2023-09-25  |http://localhost:7182/Photos/image1.jpg;http://localhost:7182/Photos/image2.jpg|desc 1|

Scenario: Retrieve lease offer list order by monthlyRent desc
	Given The following list of lease offer is present in the system
		|LeaseOfferID |LocalisationId	|Title          |Surface |Rooms|MonthlyRent|CreationDate|ConcatenedPhotos			    |Description|
		|1            |2				|Offer number 1 |65      |3    |750		 |2023-09-25  |image1.jpg;image2.jpg|desc 1|
		|2            |2				|Offer number 2 |18      |1    |950		 |2023-10-25  |image3.jpg;|desc 2|
	When I make a GET request on lease-offers endpoint with arguments 'orderBy=monthlyRent&order=Desc'
	Then The response Status code should be '200'
	And The pageable infos should be like : {CurrentPage : '1', TotalPage : '1', PageSize : '10000', totalFound : '2'}
	And The pageable content items should be like :
		|LeaseOfferID |Loc.Id|Loc.Type|Loc.Name|Loc.Hier.Id|Loc.Hier.Type|Loc.Hier.Name|Title          |Surface |Rooms|MonthlyRent|CreationDate|PhotoUris|Description|
		|2            |2	 |ARE     |Area1   |1		   |CIT			 |City1		   |Offer number 2 |18      |1    |950		 |2023-10-25  |http://localhost:7182/Photos/image3.jpg|desc 2|
		|1            |2	 |ARE     |Area1   |1	       |CIT			 |City1		   |Offer number 1 |65      |3    |750		 |2023-09-25  |http://localhost:7182/Photos/image1.jpg;http://localhost:7182/Photos/image2.jpg|desc 1|


Scenario: Retrieve lease offer list order by creationDate asc
	Given The following list of lease offer is present in the system
		|LeaseOfferID |LocalisationId	|Title          |Surface |Rooms|MonthlyRent|CreationDate|ConcatenedPhotos			    |Description|
		|1            |2				|Offer number 1 |65      |3    |950		 |2023-10-25  |image1.jpg;image2.jpg|desc 1|
		|2            |2				|Offer number 2 |18      |1    |750		 |2023-09-25  |image3.jpg;|desc 2|
	When I make a GET request on lease-offers endpoint with arguments 'orderBy=creationDate&order=Asc'
	Then The response Status code should be '200'
	And The pageable infos should be like : {CurrentPage : '1', TotalPage : '1', PageSize : '10000', totalFound : '2'}
	And The pageable content items should be like :
		|LeaseOfferID |Loc.Id|Loc.Type|Loc.Name|Loc.Hier.Id|Loc.Hier.Type|Loc.Hier.Name|Title          |Surface |Rooms|MonthlyRent|CreationDate|PhotoUris|Description|
		|2            |2	 |ARE     |Area1   |1		   |CIT			 |City1		   |Offer number 2 |18      |1    |750		 |2023-09-25  |http://localhost:7182/Photos/image3.jpg|desc 2|
		|1            |2	 |ARE     |Area1   |1	       |CIT			 |City1		   |Offer number 1 |65      |3    |950		 |2023-10-25  |http://localhost:7182/Photos/image1.jpg;http://localhost:7182/Photos/image2.jpg|desc 1|

Scenario: Retrieve lease offer list order by creationDate desc
	Given The following list of lease offer is present in the system
		|LeaseOfferID |LocalisationId	|Title          |Surface |Rooms|MonthlyRent|CreationDate|ConcatenedPhotos			    |Description|
		|1            |2				|Offer number 1 |65      |3    |950		 |2023-10-25  |image1.jpg;image2.jpg|desc 1|
		|2            |2				|Offer number 2 |18      |1    |750		 |2023-09-25  |image3.jpg;|desc 2|
	When I make a GET request on lease-offers endpoint with arguments 'orderBy=creationDate&order=Desc'
	Then The response Status code should be '200'
	And The pageable infos should be like : {CurrentPage : '1', TotalPage : '1', PageSize : '10000', totalFound : '2'}
	And The pageable content items should be like :
		|LeaseOfferID |Loc.Id|Loc.Type|Loc.Name|Loc.Hier.Id|Loc.Hier.Type|Loc.Hier.Name|Title          |Surface |Rooms|MonthlyRent|CreationDate|PhotoUris|Description|
		|1            |2	 |ARE     |Area1   |1		   |CIT			 |City1		   |Offer number 1 |65      |3    |950		 |2023-10-25  |http://localhost:7182/Photos/image1.jpg;http://localhost:7182/Photos/image2.jpg|desc 1|
		|2            |2	 |ARE     |Area1   |1	       |CIT			 |City1		   |Offer number 2 |18      |1    |750		 |2023-09-25  |http://localhost:7182/Photos/image3.jpg|desc 2|

Scenario: Retrieve lease offer list order by surface asc
	Given The following list of lease offer is present in the system
		|LeaseOfferID |LocalisationId	|Title          |Surface |Rooms|MonthlyRent|CreationDate|ConcatenedPhotos			    |Description|
		|1            |2				|Offer number 1 |65      |3    |750		 |2023-09-25  |image1.jpg;image2.jpg|desc 1|
		|2            |2				|Offer number 2 |18      |1    |950		 |2023-10-25  |image3.jpg;|desc 2|
	When I make a GET request on lease-offers endpoint with arguments 'orderBy=surface&order=Asc'
	Then The response Status code should be '200'
	And The pageable infos should be like : {CurrentPage : '1', TotalPage : '1', PageSize : '10000', totalFound : '2'}
	And The pageable content items should be like :
		|LeaseOfferID |Loc.Id|Loc.Type|Loc.Name|Loc.Hier.Id|Loc.Hier.Type|Loc.Hier.Name|Title          |Surface |Rooms|MonthlyRent|CreationDate|PhotoUris|Description|
		|2            |2	 |ARE     |Area1   |1		   |CIT			 |City1		   |Offer number 2 |18      |1    |950		 |2023-10-25  |http://localhost:7182/Photos/image3.jpg|desc 2|
		|1            |2	 |ARE     |Area1   |1	       |CIT			 |City1		   |Offer number 1 |65      |3    |750		 |2023-09-25  |http://localhost:7182/Photos/image1.jpg;http://localhost:7182/Photos/image2.jpg|desc 1|

Scenario: Retrieve lease offer list order by surface desc
	Given The following list of lease offer is present in the system
		|LeaseOfferID |LocalisationId	|Title          |Surface |Rooms|MonthlyRent|CreationDate|ConcatenedPhotos			    |Description|
		|1            |2				|Offer number 1 |18      |3    |750		 |2023-09-25  |image1.jpg;image2.jpg|desc 1|
		|2            |2				|Offer number 2 |65      |1    |950		 |2023-10-25  |image3.jpg;|desc 2|
	When I make a GET request on lease-offers endpoint with arguments 'orderBy=surface&order=Desc'
	Then The response Status code should be '200'
	And The pageable infos should be like : {CurrentPage : '1', TotalPage : '1', PageSize : '10000', totalFound : '2'}
	And The pageable content items should be like :
		|LeaseOfferID |Loc.Id|Loc.Type|Loc.Name|Loc.Hier.Id|Loc.Hier.Type|Loc.Hier.Name|Title          |Surface |Rooms|MonthlyRent|CreationDate|PhotoUris			    |Description|
		|2            |2	 |ARE     |Area1   |1		   |CIT			 |City1		   |Offer number 2 |65      |1    |950		 |2023-10-25  |http://localhost:7182/Photos/image3.jpg|desc 2|
		|1            |2	 |ARE     |Area1   |1	       |CIT			 |City1		   |Offer number 1 |18      |3    |750		 |2023-09-25  |http://localhost:7182/Photos/image1.jpg;http://localhost:7182/Photos/image2.jpg|desc 1|

Scenario: Retrieve lease offer list with both pagination and order
	Given The following list of lease offer is present in the system
		|LeaseOfferID |LocalisationId	|Title          |Surface |Rooms|MonthlyRent|CreationDate|ConcatenedPhotos			    |Description|
		|1            |2				|Offer number 1 |65      |3    |750		 |2023-09-25  |image1.jpg;image2.jpg|desc 1|
		|2            |2				|Offer number 2 |18      |1    |950		 |2023-10-25  |image3.jpg;|desc 2|
		|3            |2				|Offer number 3 |18      |1    |1000		 |2023-10-25  |image4.jpg;|desc 3|
	When I make a GET request on lease-offers endpoint with arguments 'pageSize=2&page=2&orderBy=leaseOfferId&order=Asc'
	Then The response Status code should be '200'
	And The pageable infos should be like : {CurrentPage : '2', TotalPage : '2', PageSize : '2', totalFound : '3'}
	And The pageable content items should be like :
		|LeaseOfferID |Loc.Id|Loc.Type|Loc.Name|Loc.Hier.Id|Loc.Hier.Type|Loc.Hier.Name|Title          |Surface |Rooms|MonthlyRent|CreationDate|PhotoUris|Description|
		|3            |2	 |ARE     |Area1   |1		   |CIT			 |City1		   |Offer number 3 |18      |1    |1000		 |2023-10-25  |http://localhost:7182/Photos/image4.jpg|desc 3|
	   
Scenario: Retrieve lease offer list with page index higher than max
	Given The following list of lease offer is present in the system
		|LeaseOfferID |LocalisationId	|Title          |Surface |Rooms|MonthlyRent|CreationDate|ConcatenedPhotos			    |Description|
		|1            |2				|Offer number 1 |65      |3    |750		 |2023-09-25  |image1.jpg;image2.jpg|desc 1|
		|2            |2				|Offer number 2 |18      |1    |950		 |2023-10-25  |image3.jpg;|desc 2|
	When I make a GET request on lease-offers endpoint with arguments 'pageSize=2&page=2'
	Then The response Status code should be '200'
	And The pageable infos should be like : {CurrentPage : '1', TotalPage : '1', PageSize : '2', totalFound : '2'}
	And The pageable content items should be like :
		|LeaseOfferID |Loc.Id|Loc.Type|Loc.Name|Loc.Hier.Id|Loc.Hier.Type|Loc.Hier.Name|Title          |Surface |Rooms|MonthlyRent|CreationDate|PhotoUris			    |Description|
		|2            |2	 |ARE     |Area1   |1		   |CIT			 |City1		   |Offer number 2 |18      |1    |950		 |2023-10-25  |http://localhost:7182/Photos/image3.jpg|desc 2|
		|1            |2	 |ARE     |Area1   |1	       |CIT			 |City1		   |Offer number 1 |65      |3    |750		 |2023-09-25  |http://localhost:7182/Photos/image1.jpg;http://localhost:7182/Photos/image2.jpg|desc 1|
													 						   
Scenario: Retrieve lease offer list with filter on surfaceMin
	Given The following list of lease offer is present in the system
		|LeaseOfferID |LocalisationId	|Title          |Surface |Rooms|MonthlyRent|CreationDate|ConcatenedPhotos			    |Description|
		|1            |2				|Offer number 1 |18      |3    |750		 |2023-09-25  |image1.jpg;image2.jpg|desc 1|
		|2            |2				|Offer number 2 |50      |1    |950		 |2023-10-25  |image3.jpg;|desc 2|
		|3            |2				|Offer number 2 |65      |2    |1000		 |2023-10-25  |image4.jpg;|desc 3|
	When I make a GET request on lease-offers endpoint with filter : '{"surfaceMin":19}'
	Then The response Status code should be '200'
	And The pageable infos should be like : {CurrentPage : '1', TotalPage : '1', PageSize : '10000', totalFound : '2'}
	And The pageable content items should be like :
		|LeaseOfferID |Loc.Id|Loc.Type|Loc.Name|Loc.Hier.Id|Loc.Hier.Type|Loc.Hier.Name|Title          |Surface |Rooms|MonthlyRent|CreationDate|PhotoUris			    |Description|
		|3            |2	 |ARE     |Area1   |1		   |CIT			 |City1		   |Offer number 2 |65      |2    |1000		 |2023-10-25  |http://localhost:7182/Photos/image4.jpg|desc 3|
		|2            |2	 |ARE     |Area1   |1	       |CIT			 |City1		   |Offer number 2 |50      |1    |950		 |2023-10-25  |http://localhost:7182/Photos/image3.jpg|desc 2|
																			   
Scenario: Retrieve lease offer list with filter on rooms
	Given The following list of lease offer is present in the system
		|LeaseOfferID |LocalisationId	|Title          |Surface |Rooms|MonthlyRent|CreationDate|ConcatenedPhotos			    |Description|
		|1            |2				|Offer number 1 |18      |1    |750		 |2023-09-25  |image1.jpg;image2.jpg|desc 1|
		|2            |2				|Offer number 2 |50      |2    |950		 |2023-10-25  |image3.jpg;|desc 2|
		|3            |2				|Offer number 2 |65      |3    |1000		 |2023-10-25  |image4.jpg;|desc 3|
	When I make a GET request on lease-offers endpoint with filter : '{"rooms":[2,3]}'
	Then The response Status code should be '200'
	And The pageable infos should be like : {CurrentPage : '1', TotalPage : '1', PageSize : '10000', totalFound : '2'}
	And The pageable content items should be like :
		|LeaseOfferID |Loc.Id|Loc.Type|Loc.Name|Loc.Hier.Id|Loc.Hier.Type|Loc.Hier.Name|Title          |Surface |Rooms|MonthlyRent|CreationDate|PhotoUris			    |Description|
		|3            |2	 |ARE     |Area1   |1		   |CIT			 |City1		   |Offer number 2 |65      |3    |1000		 |2023-10-25  |http://localhost:7182/Photos/image4.jpg|desc 3|
		|2            |2	 |ARE     |Area1   |1	       |CIT			 |City1		   |Offer number 2 |50      |2    |950		 |2023-10-25  |http://localhost:7182/Photos/image3.jpg|desc 2|

Scenario: Retrieve lease offer list with filter on monthlyRentMin
	Given The following list of lease offer is present in the system
		|LeaseOfferID |LocalisationId	|Title          |Surface |Rooms|MonthlyRent|CreationDate|ConcatenedPhotos			    |Description|
		|1            |2				|Offer number 1 |18      |3    |750		 |2023-09-25  |image1.jpg;image2.jpg|desc 1|
		|2            |2				|Offer number 2 |50      |1    |950		 |2023-10-25  |image3.jpg;|desc 2|
		|3            |2				|Offer number 2 |65      |2    |1000		 |2023-10-25  |image4.jpg;|desc 3|
	When I make a GET request on lease-offers endpoint with filter : '{"monthlyRentMin":751}'
	Then The response Status code should be '200'
	And The pageable infos should be like : {CurrentPage : '1', TotalPage : '1', PageSize : '10000', totalFound : '2'}
	And The pageable content items should be like :
		|LeaseOfferID |Loc.Id|Loc.Type|Loc.Name|Loc.Hier.Id|Loc.Hier.Type|Loc.Hier.Name|Title          |Surface |Rooms|MonthlyRent|CreationDate|PhotoUris			    |Description|
		|3            |2	 |ARE     |Area1   |1		   |CIT			 |City1		   |Offer number 2 |65      |2    |1000		 |2023-10-25  |http://localhost:7182/Photos/image4.jpg|desc 3|
		|2            |2	 |ARE     |Area1   |1	       |CIT			 |City1		   |Offer number 2 |50      |1    |950		 |2023-10-25  |http://localhost:7182/Photos/image3.jpg|desc 2|

Scenario: Retrieve lease offer list with filter on monthlyRentMax
	Given The following list of lease offer is present in the system
		|LeaseOfferID |LocalisationId	|Title          |Surface |Rooms|MonthlyRent|CreationDate|ConcatenedPhotos			    |Description|
		|1            |2				|Offer number 1 |18      |3    |750		 |2023-09-25  |image1.jpg;image2.jpg|desc 1|
		|2            |2				|Offer number 2 |50      |1    |950		 |2023-10-25  |image3.jpg;|desc 2|
		|3            |2				|Offer number 2 |65      |2    |1000		 |2023-10-25  |image4.jpg;|desc 3|
	When I make a GET request on lease-offers endpoint with filter : '{"monthlyRentMax":951}'
	Then The response Status code should be '200'
	And The pageable infos should be like : {CurrentPage : '1', TotalPage : '1', PageSize : '10000', totalFound : '2'}
	And The pageable content items should be like :
		|LeaseOfferID |Loc.Id|Loc.Type|Loc.Name|Loc.Hier.Id|Loc.Hier.Type|Loc.Hier.Name|Title          |Surface |Rooms|MonthlyRent|CreationDate|PhotoUris			    |Description|
		|2            |2	 |ARE     |Area1   |1		   |CIT			 |City1		   |Offer number 2 |50      |1    |950		 |2023-10-25  |http://localhost:7182/Photos/image3.jpg|desc 2|
		|1            |2	 |ARE     |Area1   |1	       |CIT			 |City1		   |Offer number 1 |18      |3    |750		 |2023-09-25  |http://localhost:7182/Photos/image1.jpg;http://localhost:7182/Photos/image2.jpg|desc 1|

Scenario: Retrieve lease offer list with filter on areas
	Given The following list of lease offer is present in the system
		|LeaseOfferID |LocalisationId	|Title          |Surface |Rooms|MonthlyRent|CreationDate|ConcatenedPhotos	|Description|
		|1            |3				|Offer number 1 |18      |3    |750		 |2023-09-25  |image1.jpg;image2.jpg|desc 1|
		|2            |2				|Offer number 2 |50      |1    |950		 |2023-10-25  |image3.jpg;			|desc 2|
		|3            |1				|Offer number 2 |65      |2    |1000	 |2023-10-25  |image4.jpg;			|desc 3|
	When I make a GET request on lease-offers endpoint with filter : '{"localisations":[2,3]}'
	Then The response Status code should be '200'
	And The pageable infos should be like : {CurrentPage : '1', TotalPage : '1', PageSize : '10000', totalFound : '2'}
	And The pageable content items should be like :
		|LeaseOfferID |Loc.Id|Loc.Type|Loc.Name|Loc.Hier.Id|Loc.Hier.Type|Loc.Hier.Name|Title          |Surface |Rooms|MonthlyRent|CreationDate|PhotoUris			    |Description|
		|2            |2	 |ARE     |Area1   |1		   |CIT			 |City1		   |Offer number 2 |50      |1    |950		 |2023-10-25  |http://localhost:7182/Photos/image3.jpg|desc 2|
		|1            |3	 |ARE     |Area2   |1	       |CIT			 |City1		   |Offer number 1 |18      |3    |750		 |2023-09-25  |http://localhost:7182/Photos/image1.jpg;http://localhost:7182/Photos/image2.jpg|desc 1|

Scenario: Retrieve lease offer list with filter on cities
	Given The following list of lease offer is present in the system
		|LeaseOfferID |LocalisationId	|Title          |Surface |Rooms|MonthlyRent|CreationDate|ConcatenedPhotos	|Description|
		|1            |3				|Offer number 1 |18      |3    |750		 |2023-09-25  |image1.jpg;image2.jpg|desc 1|
		|2            |2				|Offer number 2 |50      |1    |950		 |2023-10-25  |image3.jpg;			|desc 2|
		|3            |1				|Offer number 2 |65      |2    |1000	 |2023-10-25  |image4.jpg;			|desc 3|
	When I make a GET request on lease-offers endpoint with filter : '{"localisations":[1]}'
	Then The response Status code should be '200'
	And The pageable infos should be like : {CurrentPage : '1', TotalPage : '1', PageSize : '10000', totalFound : '3'}
	And The pageable content items should be like :
		|LeaseOfferID |Loc.Id|Loc.Type|Loc.Name|Loc.Hier.Id|Loc.Hier.Type|Loc.Hier.Name|Title          |Surface |Rooms|MonthlyRent|CreationDate|PhotoUris			    |Description|
		|3            |1	 |CIT     |City1   |		   |			 |			   |Offer number 2 |65      |2    |1000		 |2023-10-25  |http://localhost:7182/Photos/image4.jpg|desc 3|
		|2            |2	 |ARE     |Area1   |1		   |CIT			 |City1		   |Offer number 2 |50      |1    |950		 |2023-10-25  |http://localhost:7182/Photos/image3.jpg|desc 2|
		|1            |3	 |ARE     |Area2   |1	       |CIT			 |City1		   |Offer number 1 |18      |3    |750		 |2023-09-25  |http://localhost:7182/Photos/image1.jpg;http://localhost:7182/Photos/image2.jpg|desc 1|

Scenario: Retrieve lease offer list with combined filters
	Given The following list of lease offer is present in the system
		|LeaseOfferID |LocalisationId	|Title          |Surface |Rooms|MonthlyRent|CreationDate|ConcatenedPhotos			    |Description|
		|1            |2				|Offer number 1 |18      |3    |800		 |2023-09-25  |image1.jpg;image2.jpg|desc 1|
		|2            |2				|Offer number 2 |40      |1    |900		 |2023-10-25  |image3.jpg;|desc 2|
		|3            |2				|Offer number 3 |56      |2    |1000		 |2023-10-25  |image4.jpg;|desc 3|
		|4            |2				|Offer number 4 |65      |3    |1000		 |2023-11-25  |image5.jpg;|desc 4|
		|5            |2				|Offer number 5 |80      |3    |1500		 |2023-11-25  |image6.jpg;|desc 5|
	When I make a GET request on lease-offers endpoint with filter : '{"monthlyRentMin":900,"surfaceMax":64}'
	Then The response Status code should be '200'
	And The pageable infos should be like : {CurrentPage : '1', TotalPage : '1', PageSize : '10000', totalFound : '2'}
	And The pageable content items should be like :
		|LeaseOfferID |Loc.Id|Loc.Type|Loc.Name|Loc.Hier.Id|Loc.Hier.Type|Loc.Hier.Name|Title          |Surface |Rooms|MonthlyRent|CreationDate|PhotoUris			    |Description|
		|3            |2	 |ARE     |Area1   |1		   |CIT			 |City1		   |Offer number 3 |56      |2    |1000		 |2023-10-25  |http://localhost:7182/Photos/image4.jpg|desc 3|
		|2            |2	 |ARE     |Area1   |1	       |CIT			 |City1		   |Offer number 2 |40      |1    |900		 |2023-10-25  |http://localhost:7182/Photos/image3.jpg|desc 2|

Scenario: Retrieve lease offer list with both filter and pagination
	Given The following list of lease offer is present in the system
		|LeaseOfferID |LocalisationId	|Title          |Surface |Rooms|MonthlyRent|CreationDate|ConcatenedPhotos			    |Description|
		|1            |2				|Offer number 1 |18      |3    |800		 |2023-09-25  |image1.jpg;image2.jpg|desc 1|
		|2            |2				|Offer number 2 |40      |1    |900		 |2023-10-25  |image3.jpg;|desc 2|
		|3            |2				|Offer number 3 |56      |2    |950		 |2023-10-25  |image4.jpg;|desc 3|
		|4            |2				|Offer number 4 |65      |3    |1000	 |2023-11-25  |image5.jpg;|desc 4|
		|5            |2				|Offer number 5 |80      |3    |1500	 |2023-11-25  |image6.jpg;|desc 5|
	When I make a GET request on lease-offers endpoint with pagination : 'pageSize=2&page=2&orderBy=monthlyRent&order=Desc' and filter : '{"monthlyRentMin":900}'
	Then The response Status code should be '200'
	And The pageable infos should be like : {CurrentPage : '2', TotalPage : '2', PageSize : '2', totalFound : '4'}
	And The pageable content items should be like :
		|LeaseOfferID |Loc.Id|Loc.Type|Loc.Name|Loc.Hier.Id|Loc.Hier.Type|Loc.Hier.Name|Title          |Surface |Rooms|MonthlyRent|CreationDate|PhotoUris			    |Description|
		|3            |2	 |ARE     |Area1   |1		   |CIT			 |City1		   |Offer number 3 |56      |2    |950		 |2023-10-25  |http://localhost:7182/Photos/image4.jpg|desc 3|
		|2            |2	 |ARE     |Area1   |1	       |CIT			 |City1		   |Offer number 2 |40      |1    |900		 |2023-10-25  |http://localhost:7182/Photos/image3.jpg|desc 2|

Scenario: Retrieve lease offer list with filter that does not have macthing
	Given The following list of lease offer is present in the system
		|LeaseOfferID |LocalisationId	|Title          |Surface |Rooms|MonthlyRent|CreationDate|ConcatenedPhotos			    |Description|
		|1            |1				|Offer number 1 |18      |3    |800		 |2023-10-25  |image1.jpg;image2.jpg|desc 1|
		|2            |1				|Offer number 2 |40      |1    |900		 |2023-09-25  |image3.jpg;|desc 2|
	When I make a GET request on lease-offers endpoint with filter : '{"monthlyRentMax":750}'
	Then The response Status code should be '200'
	And The pageable infos should be like : {CurrentPage : '1', TotalPage : '0', PageSize : '10000', totalFound : '0'}
	And The pageable content items should be like :
		|LeaseOfferID |Title          |Surface |Rooms|MonthlyRent|CreationDate|PhotoUris			    |

Scenario: Retrieve lease offer list with wrong numeric filter field value
	Given Whatever data I have in the system
	When I make a GET request on lease-offers endpoint with filter : '{"surfaceMin":\'WrongValue\'}'
	Then The response Status code should be '400'

Scenario: Retrieve lease offer list with wrong page size
	Given Whatever data I have in the system
	When I make a GET request on lease-offers endpoint with arguments 'pageSize=-1'
	Then The response Status code should be '400'

Scenario: Retrieve lease offer list with wrong order field name
	Given Whatever data I have in the system
	When I make a GET request on lease-offers endpoint with arguments 'orderBy=unknown'
	Then The response Status code should be '400'

Scenario: Retrieve lease offer list with wrong order value
	Given Whatever data I have in the system
	When I make a GET request on lease-offers endpoint with arguments 'orderBy=leaseOfferId&order=ascendant'
	Then The response Status code should be '400'

Scenario: Retrieve lease offer list with order but no order field
	Given Whatever data I have in the system
	When I make a GET request on lease-offers endpoint with arguments 'order=Asc'
	Then The response Status code should be '400'