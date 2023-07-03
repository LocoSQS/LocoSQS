Feature: TagQueue

Scenario: When a queue is tagged, that tag exists on the queue
    Given a queue
    And the queue has a tag with name Test and value Hello
    Then the queue has 1 tag
    And the queue has a tag with name Test and value Hello
   
Scenario: When a queue is tagged then untagged, that tag no longer exists on the queue
    Given a queue
    And the queue has a tag with name Test and value Hello
    When i remove a tag with name Test from the queue
    Then the queue does not have a tag with name Test
    And the queue has 0 tags
    
Scenario: When a queue is tagged multiple times then one is removed, the other tags exist on the queue
    Given a queue
    And the queue has a tag with name Test and value Hello
    And the queue has a tag with name Test2 and value Hello2
    And the queue has a tag with name Test3 and value Hello3
    When i remove a tag with name Test2 from the queue
    Then the queue has 2 tags
    And the queue has a tag with name Test and value Hello
    And the queue has a tag with name Test3 and value Hello3