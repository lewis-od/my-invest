Feature: Cash
	Adding and withdrawing cash to and from accounts

Scenario: Add cash to open account
	Given a client has signed up
	And their address is verified
	And they have a GIA account with status Open
	When they add £200.00 cash to their account
	Then the account has balance £200.00

Scenario: Add cash to pre-open account
	Given a client has signed up
	And they have a GIA account with status PreOpen
	When they add £200.00 cash to their account
	Then they receive a conflict error code
	And the account has balance £0.00
