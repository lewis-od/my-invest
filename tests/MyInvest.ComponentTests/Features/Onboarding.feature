Feature: Onboarding
	Users signing up for the platform

Scenario: Sign up
	Given my username is test-user
	When I sign up
	Then I get assigned a user ID
	And I have 0 investment accounts

Scenario: Opening an account
	Given I have a MyInvest profile
	When I open a GIA account
	Then an account with type GIA is created
	And the account is assigned an ID
	And the account has status PreOpen
	And the account has balance £0.00
