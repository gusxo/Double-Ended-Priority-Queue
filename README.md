# Double-Ended-Priority-Queue
C# Implements of Priority-Queue and Double-Ended-Priority-Queue.

### namespace : PriorityQueue
PriorityQueue.PriorityQueue<T>  
PriorityQueue.DEPriorityQueue<T>  
PriorityQueue.Priority  

### PriorityQueue\<T> Class  
Implements of Priority-Queue.  
Supports public method Enqueue(T element), Dequeue(), Front() and Count public property.  
Priority is determined by the **Compare\<T> compareMethod** of the Constructor.  

### delegate bool Compare\<T>(T a, T b)  
element compare method. if it returns true, the prioirty of 'b' is higher.  

### DEPriorityQueue\<T> Class  
Implements of Double-Ended-Priority-Queue.  
Supports public method Enqueue(T element), Dequeue(Priority select), Front(Priority select) and Count public property.  
In Front() and Dequeue(), use Priority Enum(Only Contains 'Highest' and 'Lowest') to select the highest priority element or lowest priority element.  
Priority is determined by the **Compare\<T> compareMethod** of the Constructor.  

- - -
## Example
In the example below, the higher the value, the higher the priority.  
```
PriorityQueue.PriorityQueue<int> pq = new PriorityQueue.PriorityQueue<int>( (a,b) => {return a < b;} );
pq.Enqueue(2);
pq.Dequeue();
                                                                                                    
PriorityQueue.DEPriorityQueue<int> depq = new PriorityQueue.DEPriorityQueue<int>( (a,b) => {return a < b;} );
depq.Enqueue(2);
depq.Enqueue(3);
depq.Dequeue(PriorityQueue.Priority.Highest);
                                                                                                          
```
In the example below, the higher the value, the lower the priority.  
```
PriorityQueue.PriorityQueue<int> pq = new PriorityQueue.PriorityQueue<int>( (a,b) => {return a > b;} );
pq.Enqueue(2);
pq.Dequeue();
  
PriorityQueue.DEPriorityQueue<int> depq = new PriorityQueue.DEPriorityQueue<int>( (a,b) => {return a > b;} );
depq.Enqueue(2);
depq.Enqueue(3);
depq.Dequeue(PriorityQueue.Priority.Lowest);
```
