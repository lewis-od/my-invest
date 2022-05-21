Feature: Back office
	Operations performed by the back office team

Scenario: Verify address for client with pre-open account
	Given a client has signed up
	And they have a GIA account with status PreOpen
	When their address is verified by the back office team
	Then the account has status Open
