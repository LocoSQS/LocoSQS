Feature: ChangeMessageVisibility

A short summary of the feature

Scenario: When a message is read then made visible again, it should be able to be read
	Given a queue
	And there is a message on the queue
	And the queue has an attribute called VisibilityTimeout with value 3600
	When i ask for 1 message
	And i make the first message received visible again
	And i ask for 1 message
	Then i get 1 message

Scenario: When many messages are read then made visible again, it should be able to be read all the messages
	Given a queue
	And there are 10 messages on the queue
	And the queue has an attribute called VisibilityTimeout with value 3600
	When i exhaust the queue by constantly reading messages
	And i make all messages received visible again
	And i exhaust the queue by constantly reading messages
	Then i get 10 messages