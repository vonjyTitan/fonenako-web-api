Feature: Find many lease offers
	In order to obtain a leas offer list

Scenario: Retrieve lease offer list without any parameter
	Given The following list of lease offer is present in the system
		|LeaseOfferID |Title          |Surface |Rooms|MonthlyRent|
		|1            |Offer number 1 |65      |3    |750		 |
		|2            |Offer number 2 |18      |1    |950		 |
	When I make a GET request on lease-offers endpoint without any argument
	Then The response Status code should be '200'
	And The pageable infos should be like : {CurrentPage : '1', TotalPage : '1', PageSize : '10000'}
	And The pageable content items should be like :
		|LeaseOfferID |Title          |Surface |Rooms|MonthlyRent|
		|2            |Offer number 2 |18      |1    |950		 |
		|1            |Offer number 1 |65      |3    |750		 |

Scenario: Retrieve lease offer list order by leaseOfferId asc
	Given The following list of lease offer is present in the system
		|LeaseOfferID |Title          |Surface |Rooms|MonthlyRent|
		|1            |Offer number 1 |65      |3    |950		 |
		|2            |Offer number 2 |18      |1    |750		 |
	When I make a GET request on lease-offers endpoint with arguments 'orderBy=leaseOfferId&order=Asc'
	Then The response Status code should be '200'
	And The pageable infos should be like : {CurrentPage : '1', TotalPage : '1', PageSize : '10000'}
	And The pageable content items should be like :
		|LeaseOfferID |Title          |Surface |Rooms|MonthlyRent|
		|1            |Offer number 1 |65      |3    |950		 |
		|2            |Offer number 2 |18      |1    |750		 |

Scenario: Retrieve lease offer list order by leaseOfferId desc
	Given The following list of lease offer is present in the system
		|LeaseOfferID |Title          |Surface |Rooms|MonthlyRent|
		|1            |Offer number 1 |65      |3    |750		 |
		|2            |Offer number 2 |18      |1    |950		 |
	When I make a GET request on lease-offers endpoint with arguments 'orderBy=leaseOfferId&order=Desc'
	Then The response Status code should be '200'
	And The pageable infos should be like : {CurrentPage : '1', TotalPage : '1', PageSize : '10000'}
	And The pageable content items should be like :
		|LeaseOfferID |Title          |Surface |Rooms|MonthlyRent|
		|2            |Offer number 2 |18      |1    |950		 |
		|1            |Offer number 1 |65      |3    |750		 |

Scenario: Retrieve lease offer list order by monthlyRent asc
	Given The following list of lease offer is present in the system
		|LeaseOfferID |Title          |Surface |Rooms|MonthlyRent|
		|1            |Offer number 1 |65      |3    |950		 |
		|2            |Offer number 2 |18      |1    |750		 |
	When I make a GET request on lease-offers endpoint with arguments 'orderBy=monthlyRent&order=Asc'
	Then The response Status code should be '200'
	And The pageable infos should be like : {CurrentPage : '1', TotalPage : '1', PageSize : '10000'}
	And The pageable content items should be like :
		|LeaseOfferID |Title          |Surface |Rooms|MonthlyRent|
		|2            |Offer number 2 |18      |1    |750		 |
		|1            |Offer number 1 |65      |3    |950		 |

Scenario: Retrieve lease offer list order by monthlyRent desc
	Given The following list of lease offer is present in the system
		|LeaseOfferID |Title          |Surface |Rooms|MonthlyRent|
		|1            |Offer number 1 |65      |3    |750		 |
		|2            |Offer number 2 |18      |1    |950		 |
	When I make a GET request on lease-offers endpoint with arguments 'orderBy=monthlyRent&order=Desc'
	Then The response Status code should be '200'
	And The pageable infos should be like : {CurrentPage : '1', TotalPage : '1', PageSize : '10000'}
	And The pageable content items should be like :
		|LeaseOfferID |Title          |Surface |Rooms|MonthlyRent|
		|2            |Offer number 2 |18      |1    |950		 |
		|1            |Offer number 1 |65      |3    |750		 |

Scenario: Retrieve lease offer list order by surface asc
	Given The following list of lease offer is present in the system
		|LeaseOfferID |Title          |Surface |Rooms|MonthlyRent|
		|1            |Offer number 1 |65      |3    |750		 |
		|2            |Offer number 2 |18      |1    |950		 |
	When I make a GET request on lease-offers endpoint with arguments 'orderBy=surface&order=Asc'
	Then The response Status code should be '200'
	And The pageable infos should be like : {CurrentPage : '1', TotalPage : '1', PageSize : '10000'}
	And The pageable content items should be like :
		|LeaseOfferID |Title          |Surface |Rooms|MonthlyRent|
		|2            |Offer number 2 |18      |1    |950		 |
		|1            |Offer number 1 |65      |3    |750		 |

Scenario: Retrieve lease offer list order by surface desc
	Given The following list of lease offer is present in the system
		|LeaseOfferID |Title          |Surface |Rooms|MonthlyRent|
		|1            |Offer number 1 |18      |3    |750		 |
		|2            |Offer number 2 |65      |1    |950		 |
	When I make a GET request on lease-offers endpoint with arguments 'orderBy=surface&order=Desc'
	Then The response Status code should be '200'
	And The pageable infos should be like : {CurrentPage : '1', TotalPage : '1', PageSize : '10000'}
	And The pageable content items should be like :
		|LeaseOfferID |Title          |Surface |Rooms|MonthlyRent|
		|2            |Offer number 2 |65      |1    |950		 |
		|1            |Offer number 1 |18      |3    |750		 |

Scenario: Retrieve lease offer list with both pagination and order
	Given The following list of lease offer is present in the system
		|LeaseOfferID |Title          |Surface |Rooms|MonthlyRent|
		|1            |Offer number 1 |65      |3    |750		 |
		|2            |Offer number 2 |18      |1    |950		 |
		|3            |Offer number 3 |18      |1    |1000		 |
	When I make a GET request on lease-offers endpoint with arguments 'pageSize=2&page=2&orderBy=leaseOfferId&order=Asc'
	Then The response Status code should be '200'
	And The pageable infos should be like : {CurrentPage : '2', TotalPage : '2', PageSize : '2'}
	And The pageable content items should be like :
		|LeaseOfferID |Title          |Surface |Rooms|MonthlyRent|
		|3            |Offer number 3 |18      |1    |1000		 |

Scenario: Retrieve lease offer list with page index higher than max
	Given The following list of lease offer is present in the system
		|LeaseOfferID |Title          |Surface |Rooms|MonthlyRent|
		|1            |Offer number 1 |65      |3    |750		 |
		|2            |Offer number 2 |18      |1    |950		 |
	When I make a GET request on lease-offers endpoint with arguments 'pageSize=2&page=2'
	Then The response Status code should be '200'
	And The pageable infos should be like : {CurrentPage : '1', TotalPage : '1', PageSize : '2'}
	And The pageable content items should be like :
		|LeaseOfferID |Title          |Surface |Rooms|MonthlyRent|
		|2            |Offer number 2 |18      |1    |950		 |
		|1            |Offer number 1 |65      |3    |750		 |
													 
Scenario: Retrieve lease offer list with filter on surfaceMin
	Given The following list of lease offer is present in the system
		|LeaseOfferID |Title          |Surface |Rooms|MonthlyRent|
		|1            |Offer number 1 |18      |3    |750		 |
		|2            |Offer number 2 |50      |1    |950		 |
		|3            |Offer number 2 |65      |2    |1000		 |
	When I make a GET request on lease-offers endpoint with filter : 'surfaceMin=19'
	Then The response Status code should be '200'
	And The pageable infos should be like : {CurrentPage : '1', TotalPage : '1', PageSize : '10000'}
	And The pageable content items should be like :
		|LeaseOfferID |Title          |Surface |Rooms|MonthlyRent|
		|3            |Offer number 2 |65      |2    |1000		 |
		|2            |Offer number 2 |50      |1    |950		 |

Scenario: Retrieve lease offer list with filter on roomsMax
	Given The following list of lease offer is present in the system
		|LeaseOfferID |Title          |Surface |Rooms|MonthlyRent|
		|1            |Offer number 1 |18      |1    |750		 |
		|2            |Offer number 2 |50      |2    |950		 |
		|3            |Offer number 2 |65      |3    |1000		 |
	When I make a GET request on lease-offers endpoint with filter : 'roomsMax=2'
	Then The response Status code should be '200'
	And The pageable infos should be like : {CurrentPage : '1', TotalPage : '1', PageSize : '10000'}
	And The pageable content items should be like :
		|LeaseOfferID |Title          |Surface |Rooms|MonthlyRent|
		|2            |Offer number 2 |50      |2    |950		 |
		|1            |Offer number 1 |18      |1    |750		 |

Scenario: Retrieve lease offer list with filter on roomsMin
	Given The following list of lease offer is present in the system
		|LeaseOfferID |Title          |Surface |Rooms|MonthlyRent|
		|1            |Offer number 1 |18      |1    |750		 |
		|2            |Offer number 2 |50      |2    |950		 |
		|3            |Offer number 2 |65      |3    |1000		 |
	When I make a GET request on lease-offers endpoint with filter : 'roomsMin=2'
	Then The response Status code should be '200'
	And The pageable infos should be like : {CurrentPage : '1', TotalPage : '1', PageSize : '10000'}
	And The pageable content items should be like :
		|LeaseOfferID |Title          |Surface |Rooms|MonthlyRent|
		|3            |Offer number 2 |65      |3    |1000		 |
		|2            |Offer number 2 |50      |2    |950		 |

Scenario: Retrieve lease offer list with filter on monthlyRentMin
	Given The following list of lease offer is present in the system
		|LeaseOfferID |Title          |Surface |Rooms|MonthlyRent|
		|1            |Offer number 1 |18      |3    |750		 |
		|2            |Offer number 2 |50      |1    |950		 |
		|3            |Offer number 2 |65      |2    |1000		 |
	When I make a GET request on lease-offers endpoint with filter : 'monthlyRentMin=751'
	Then The response Status code should be '200'
	And The pageable infos should be like : {CurrentPage : '1', TotalPage : '1', PageSize : '10000'}
	And The pageable content items should be like :
		|LeaseOfferID |Title          |Surface |Rooms|MonthlyRent|
		|3            |Offer number 2 |65      |2    |1000		 |
		|2            |Offer number 2 |50      |1    |950		 |

Scenario: Retrieve lease offer list with filter on monthlyRentMax
	Given The following list of lease offer is present in the system
		|LeaseOfferID |Title          |Surface |Rooms|MonthlyRent|
		|1            |Offer number 1 |18      |3    |750		 |
		|2            |Offer number 2 |50      |1    |950		 |
		|3            |Offer number 2 |65      |2    |1000		 |
	When I make a GET request on lease-offers endpoint with filter : 'monthlyRentMax=951'
	Then The response Status code should be '200'
	And The pageable infos should be like : {CurrentPage : '1', TotalPage : '1', PageSize : '10000'}
	And The pageable content items should be like :
		|LeaseOfferID |Title          |Surface |Rooms|MonthlyRent|
		|2            |Offer number 2 |50      |1    |950		 |
		|1            |Offer number 1 |18      |3    |750		 |

Scenario: Retrieve lease offer list with combined filters
	Given The following list of lease offer is present in the system
		|LeaseOfferID |Title          |Surface |Rooms|MonthlyRent|
		|1            |Offer number 1 |18      |3    |800		 |
		|2            |Offer number 2 |40      |1    |900		 |
		|3            |Offer number 3 |56      |2    |1000		 |
		|4            |Offer number 4 |65      |3    |1000		 |
		|5            |Offer number 5 |80      |3    |1500		 |
	When I make a GET request on lease-offers endpoint with filter : 'monthlyRentMin=900&surfaceMax=64'
	Then The response Status code should be '200'
	And The pageable infos should be like : {CurrentPage : '1', TotalPage : '1', PageSize : '10000'}
	And The pageable content items should be like :
		|LeaseOfferID |Title          |Surface |Rooms|MonthlyRent|
		|3            |Offer number 3 |56      |2    |1000		 |
		|2            |Offer number 2 |40      |1    |900		 |

Scenario: Retrieve lease offer list with both filter and pagination
	Given The following list of lease offer is present in the system
		|LeaseOfferID |Title          |Surface |Rooms|MonthlyRent|
		|1            |Offer number 1 |18      |3    |800		 |
		|2            |Offer number 2 |40      |1    |900		 |
		|3            |Offer number 3 |56      |2    |950		 |
		|4            |Offer number 4 |65      |3    |1000		 |
		|5            |Offer number 5 |80      |3    |1500		 |
	When I make a GET request on lease-offers endpoint with pagination : 'pageSize=2&page=2&orderBy=monthlyRent&order=Desc' and filter : 'monthlyRentMin=900'
	Then The response Status code should be '200'
	And The pageable infos should be like : {CurrentPage : '2', TotalPage : '2', PageSize : '2'}
	And The pageable content items should be like :
		|LeaseOfferID |Title          |Surface |Rooms|MonthlyRent|
		|3            |Offer number 3 |56      |2    |950		 |
		|2            |Offer number 2 |40      |1    |900		 |

Scenario: Retrieve lease offer list with filter that does not have macthing
	Given The following list of lease offer is present in the system
		|LeaseOfferID |Title          |Surface |Rooms|MonthlyRent|
		|1            |Offer number 1 |18      |3    |800		 |
		|2            |Offer number 2 |40      |1    |900		 |
	When I make a GET request on lease-offers endpoint with filter : 'monthlyRentMax=750'
	Then The response Status code should be '200'
	And The pageable infos should be like : {CurrentPage : '1', TotalPage : '0', PageSize : '10000'}
	And The pageable content items should be like :
		|LeaseOfferID |Title          |Surface |Rooms|MonthlyRent|

Scenario: Retrieve lease offer list with wrong filter field name
	Given Whatever data I have in the system
	When I make a GET request on lease-offers endpoint with filter : 'uknownFieldName=18'
	Then The response Status code should be '400'

Scenario: Retrieve lease offer list with wrong numeric filter field value
	Given Whatever data I have in the system
	When I make a GET request on lease-offers endpoint with filter : 'surfaceMin=WrongValue'
	Then The response Status code should be '400'

Scenario: Retrieve lease offer list with empty filter field value
	Given Whatever data I have in the system
	When I make a GET request on lease-offers endpoint with filter : 'surfaceMin='
	Then The response Status code should be '400'

Scenario: Retrieve lease offer list with filter field name only
	Given Whatever data I have in the system
	When I make a GET request on lease-offers endpoint with filter : 'surfaceMin'
	Then The response Status code should be '400'

Scenario: Retrieve lease offer list with duplicate filter field
	Given Whatever data I have in the system
	When I make a GET request on lease-offers endpoint with filter : 'surfaceMin=18&surfaceMin=60'
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