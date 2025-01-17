# eCom

## Inline vs Async vs Live Projections

### Reading with querySession.Json.WriteById
Should be only used with Inline projections!
Async projections are not stored in the database as read models, 
for async projections you need to do the aggregation in the get endpoint
