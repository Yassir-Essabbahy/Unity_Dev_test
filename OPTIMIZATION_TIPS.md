# Performance Optimization Notes

## 1. Implemented Optimizations

* **Object pooling:** Reused bullets and explosion effects instead of repeatedly creating and destroying them.
* **Removed log spam:** Removed debug logging from the enemy AI's `FixedUpdate()` and replaced string tag comparisons with `CompareTag()`.
* **Physics timing:** Used `Time.fixedDeltaTime` for Rigidbody movement and rotation inside `FixedUpdate()`.
* **Debug panel:** Reduced unnecessary UI updates and scene searches by refreshing the displayed stats every 0.15 seconds.

## 2. Further Optimization Ideas

* **Rendering:** Use static batching, GPU instancing and shared materials where appropriate to reduce draw calls.
* **Lighting:** Bake lighting for static objects and reduce shadow distance if needed.
* **Physics:** Disable unnecessary layer collisions and use appropriate collision detection modes.
* **Textures:** Use platform-appropriate compression and mipmaps for 3D textures.
* **Memory:** Avoid unnecessary allocations in frequently called methods and reuse collections where possible.

## 3. Profiling Checklist

Before release we need to use Unity Profiler to check:

* CPU and GPU frame times.
* Garbage collection during combat.
* Draw calls and SetPass calls.
* Physics performance with multiple enemies and projectiles.


