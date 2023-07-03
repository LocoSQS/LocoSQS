Feature: PurgeQueue

A short summary of the feature

Scenario: Does purging a queue remove its messages
	Given a queue
	And there is a message on the queue
	When i purge the queue
	# You are supposed to wait longer, but usually it'll still work
	And i wait for 10 seconds 
	And i ask for 10 messages
	Then i get 0 messages
