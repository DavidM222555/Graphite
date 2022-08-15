# Graphite
Graphite is a NoSQL database language based off the principles and ideas of a knowledge graph. Currently, Graphite has support for the basic operations of creating a graph,
adding properties to named nodes in the graph, adding relationships between named nodes in the graph, and most importantly it allows some basic querying to find nodes
that have certain properties and relationships to other nodes.

## Query Syntax
### Create a graph
CREATE \<graphname\>

### Add a node to a graph
ADD \<node\> IN \<graphname\>

### Add properties to given nodes
GIVE \<node1\>,\<node2\>,... PROPS \<prop1\>,\<prop2\>,... IN \<graphname\>

### Relate nodes 
RELATE \<node1\> TO \<node2\> WITH \<relationship\> IN \<graphname\> 

### Query nodes with property
GET NODES WITH PROPS \<prop1\>,\<prop2\>,... IN \<graphname\>

## Future Work
- Allow for the creation of type based properties, such as integer properties, that can then be tested quantitatively instead of just qualitatively (for example, get all people in a graph that are above a certain height).
- Allow for certain operations to join two graphs that meet certain conditions. Consider the following example, we have two graphs, one is a graph of students and another is a group of workers, and now we want to create a graph that consists of student workers (thus we find all nodes that can be found in both graphs and create a new graph with all those nodes).
