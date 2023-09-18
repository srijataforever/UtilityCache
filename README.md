# Utility Cache

#### Task: Caching REST API built to cache TKey and TValue

You have been asked to create a generic in-memory cache component, which other FINBOURNE
developers can use in their applications.
This component should be able to store arbitrary types of objects, which are added and retrieved
using a unique key (similar to a dictionary).
To avoid the risk of running out of memory, the cache will need to have a configurable threshold for
the maximum number of items which it can hold at any one time. If the cache becomes full, any
attempts to add additional items should succeed, but will result in another item in the cache being
evicted. The cache should implement the ‘least recently used’ approach when selecting which item
to evict.
The cache component is intended to be used as a singleton. As such, you should ideally make your
component thread-safe for all methods, but you can skip this feature if you run out of time.
Another useful feature would be some kind of mechanism which allows the consumer to know when
items get evicted. Again, if you run out of time, you can skip this feature too.
Constraints
• Please write the solution in C# and .NET
• You may use any .NET framework version you wish
• You may use any development tools you wish
• You are permitted to use external libraries (e.g. nuget packages)

#### Problems faced: 
  * Building a generic controller - Ran into 'No operations defined in spec!' issue

#### Resolution: 
  * Using string Key and object Value (although object requires boxing and unboxing)
