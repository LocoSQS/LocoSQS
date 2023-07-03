Feature: ReceiveMessage

A short summary of the feature

Scenario: A message that is sent to a queue can be retrieved
	Given a queue
	And there is a message on the queue
	When i ask for 10 messages
	Then i get 1 message

Scenario: A message that is sent to a queue, then retrieved, will be invisible
	Given a queue
	And there is a message on the queue
	And the queue has an attribute called VisibilityTimeout with value 3
	When i ask for 10 messages
	And i ask for 10 messages
	Then i get 0 messages
	
Scenario: A message that is sent to a queue, waited for, then retrieved, should be visible
	Given a queue
	And there is a message on the queue
	And the queue has an attribute called VisibilityTimeout with value 3
	When i ask for 10 messages
	And i wait for 5 seconds
	And i ask for 10 messages
	Then i get 1 message

Scenario: A message that is sent to the queue has the same text when it is recieved
	Given a queue
	And there is a message with text Hello! on the queue
	When i ask for 10 messages
	Then i get 1 message
	And any message has Hello! as text

Scenario: Attributes are not sent along when they are not requested
	Given a queue
	And there is a message on the queue
	When i ask for 1 messages silently
	Then i get 1 message
	And no message has an attribute called ApproximateReceiveCount

Scenario: The readcount of a message increases every time it is read
	Given a queue
	And there is a message on the queue
	When i ask for 1 messages silently
	And i ask for 1 message with attributes
	Then i get 1 message
	And any message has an attribute with value 2 called ApproximateReceiveCount
	
Scenario: Messages have user defined attributes that are stored
	Given a queue
	And there is a message with a user defined attribute called TestAttribute with type String and value Hello on the queue
	When i ask for 1 message with attributes
	Then i get 1 message
	And any message has an user defined attribute called TestAttribute of type String with value Hello
	
Scenario: Messages only give back user defined attributes if they are requested
	Given a queue
	And there is a message with a user defined attribute called TestAttribute with type String and value Hello on the queue
	When i ask for 1 message
	Then i get 1 message
	And no messages have an user defined attribute called TestAttribute