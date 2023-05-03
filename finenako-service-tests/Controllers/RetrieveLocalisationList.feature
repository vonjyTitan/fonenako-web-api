Feature: Retrieve localisations list

Background:
	Given The following list of localisations is present in the system
		|LocalisationId|Type  |HierarchyId|Name			|
		|1			   |CIT   |			  |Antananarivo |
		|2			   |CIT   |			  |Atsimon-drano|
		|3			   |ARE   | 2		  |Tanjombato	|
		|4			   |ARE   | 1		  |Analakely	|

Scenario: Search localisations with name that contains a word
	When I make a GET request on localisations api with parameters : 'name=Tan'
	Then The response Status code should be '200'
	And The pageable localisation infos should be like : {CurrentPage : '1', TotalPage : '1', PageSize : '10000', totalFound : '2'}
	And The response content as localisations list should be like :
	|LocalisationId|Type|Name          |Hier.Id|Hier.Type|Hier.Name	   |
	|1			   |CIT |Antananarivo  |	   |	     |			   |
	|3			   |ARE |Tanjombato    |2	   |CIT	     |Atsimon-drano|

Scenario: Search localisations without filter value
	When I make a GET request on localisations api with parameters : 'name='
	Then The response Status code should be '200'
	And The pageable localisation infos should be like : {CurrentPage : '1', TotalPage : '1', PageSize : '10000', totalFound : '4'}
	And The response content as localisations list should be like :
	|LocalisationId|Type|Name          |Hier.Id|Hier.Type|Hier.Name	   |
	|4			   |ARE |Analakely     |1	   |CIT	     |Antananarivo |
	|1			   |CIT |Antananarivo  |	   |	     |			   |
	|2			   |CIT |Atsimon-drano |	   |	     |			   |
	|3			   |ARE |Tanjombato    |2	   |CIT	     |Atsimon-drano|

Scenario: Retrieve localisations list with wrong page size
	Given Whatever data I have in the system
	When I make a GET request on localisations api with parameters : 'pageSize=-1'
	Then The response Status code should be '400'

Scenario: Retrieve localisations list with wrong order field name
	Given Whatever data I have in the system
	When I make a GET request on localisations api with parameters : 'orderBy=unknown'
	Then The response Status code should be '400'

Scenario: Retrieve localisations list with wrong order value
	Given Whatever data I have in the system
	When I make a GET request on localisations api with parameters : 'orderBy=Name&order=ascendant'
	Then The response Status code should be '400'

Scenario: Retrieve localisations list with order but no order field
	Given Whatever data I have in the system
	When I make a GET request on localisations api with parameters : 'order=Asc'
	Then The response Status code should be '400'