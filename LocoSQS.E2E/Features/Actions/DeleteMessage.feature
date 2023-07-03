Feature: DeleteMessage

A short summary of the feature

Scenario: When a message is deleted, it is removed from the queue
	Given a queue
	And there is a message on the queue
	When i ask for 1 message silently
	And i delete the first received message
	And i ask for 1 message
	Then i get 0 messages

Scenario: When messages are deleted in bulk, they are all removed from the queue. Tested by estimating how many messages are on the queue
	Given a queue
	And there are 10 messages on the queue
	When i ask for 10 message silently
	And i delete all received messages
	And i wait for 10 seconds
	Then there are less than 10 messages on the queue

Scenario: When messages are deleted in bulk, they are all removed from the queue. Tested by receiving all messages
	Given a queue
	And there are 10 messages on the queue
	When i exhaust the queue by constantly reading messages
	And i delete all received messages
	And i ask for 10 messages
	Then i get 0 messages