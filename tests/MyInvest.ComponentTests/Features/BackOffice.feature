Feature: Back office
	Operations performed by the back office team

Scenario: Add two numbers
	Given a client exists
	And they have a GIA account with status PreOpen
	When I verify their address
	Then the account has status Open
	