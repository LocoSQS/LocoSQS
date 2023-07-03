Feature: GetQueueAttributes

A short summary of the feature

Scenario: The queue can read how many messages it has correctly
	Given a queue
	And there are 9 messages on the queue
	When i wait for 10 seconds
	Then there are 9 messages on the queue
	And there are less than 10 messages on the queue