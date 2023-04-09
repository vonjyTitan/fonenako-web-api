Feature: Retrieve list of cities

Background:
	Given The following list of city is present in the system
		|CityId|Name  |
		|1	   | City1|
		|2	   | City2|
	Given The following list of area is present in the system
		|AreaId|CityId|Name  |
		|1	   |1     | Area1|
		|2	   |2     | Area2|
		|3	   |2     | Area3|

Scenario: Retrieve cities without any argument
	Given The city and area default values are present in the system 
	When I make a GET request on cities endpoint without any argument
	Then The response Status code should be '200'
	And The pageable city infos should be like : {CurrentPage : '1', TotalPage : '1', PageSize : '10000', totalFound : '2'}
	And The pageable city content items should be like :
	|CityId|Name |AreasId|AreasName	 |
	|1	   |City1|1		 |Area1	     |
	|2	   |City2|2,3	 |Area2,Area3|

Scenario: Retrieve cities with filter
	Given The city and area default values are present in the system 
	When I make a GET request on cities endpoint with filter '{"name":"ty1"}'
	Then The response Status code should be '200'
	And The pageable city infos should be like : {CurrentPage : '1', TotalPage : '1', PageSize : '10000', totalFound : '1'}
	And The pageable city content items should be like :
	|CityId|Name |AreasId|AreasName	 |
	|1	   |City1|1		 |Area1	     |

Scenario: Retrieve cities with order order by name
	Given The city and area default values are present in the system 
	When I make a GET request on cities endpoint with pagination 'orderBy=name&order=Asc'
	Then The response Status code should be '200'
	And The pageable city infos should be like : {CurrentPage : '1', TotalPage : '1', PageSize : '10000', totalFound : '2'}
	And The pageable city content items should be like :
	|CityId|Name |AreasId|AreasName	 |
	|1	   |City1|1		 |Area1	     |
	|2	   |City2|2,3	 |Area2,Area3|

Scenario: Retrieve cities with wrong order field name
	Given The city and area default values are present in the system 
	When I make a GET request on cities endpoint with pagination 'orderBy=unknownField'
	Then The response Status code should be '400'

Scenario: Retrieve cities with wrong sort direction value
	Given The city and area default values are present in the system 
	When I make a GET request on cities endpoint with pagination 'orderBy=name&order=Descendant'
	Then The response Status code should be '400'

Scenario: Retrieve cities with sort direction while order field is empty
	Given The city and area default values are present in the system 
	When I make a GET request on cities endpoint with pagination 'orderBy=&order=Asc'
	Then The response Status code should be '400'


Scenario: Retrieve cities with order field while sort direction is empty
	Given The city and area default values are present in the system 
	When I make a GET request on cities endpoint with pagination 'orderBy=name'
	Then The response Status code should be '400'
