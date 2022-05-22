Feature: Onboarding
	Users signing up for the platform

Scenario: Sign up
	Given a client has username test-user
	When they sign up for a profile
	Then the request is successful
	And they are assigned a user ID
	And they have 0 investment accounts

Scenario: Opening an account
	Given a client has signed up
	When they open a GIA account
	Then the request is successful
	And an account with type GIA is created
	And the account is assigned an ID
	And the account has status PreOpen
	And the account has balance £0.00
