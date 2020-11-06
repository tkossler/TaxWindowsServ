# TaxWindowsServ
Windows Service to call Tax Jar Service

There are 3 projects in this solution

1. TaxService:  This is a standard WCF windows service built in Visual Studio Enterprise 2017 using .NET framework 4.6
	2 Methods
	1. GetRateFromZip(string usZipCode) pass in a US Zip Code to get a standard Tax Jar RateResponse object back
	2. GetTaxForOrder(Order order) pass in a Tax Jar order object and get back a Tax Jar TaxRepsonseAttribute object back

2.   UNit Testing:  There are 2 UNit Test projects
	1. UNitTestingWCFservice for testing the WCF service calls
		a. Update ConnectedService.json file with corrrect URL to be used for testing
		b. Current url for testing is set to http://localhost:50728/TaxCalculator.svc 


	2. UNitTestTaxJar Direct call
		a.  Makes 2 direct REST calls to "https://api.taxjar.com/v2/";
	