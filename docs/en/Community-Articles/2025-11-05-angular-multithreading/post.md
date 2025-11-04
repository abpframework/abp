# Angular Multithreading: When to Use Web Workers vs Shared Workers

Modern web applications demand seamless user experiences, even when handling heavy computational tasks. Developers often face performance bottlenecks when heavy processing blocks the main thread, causing UI freezes and poor responsiveness.
Web Workers and Shared Workers—two powerful browser APIs that enable true multithreading in web applications. While both move heavy computation off the main thread, they serve distinctly different purposes. This article explores when to use each type and provides practical implementation patterns in Angular.

### Understanding Main Thread Problem

The main thread is single-thread, meaning its handle only one task at a time. When heavy computation occurs on the main thread it blocks other critical tasks such as rendering and event handling. This leads to a poor user experience with UI freezes and unresponsiveness.

````typescript

// Example of blocking main thread
function heavyComputation() {
  let sum = 0;
  for (let i = 0; i < 1e9; i++) {
    sum += i;
  }
  return sum;
}  
console.log(heavyComputation()); // Blocks UI until done
````