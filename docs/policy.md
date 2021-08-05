# Container Placement Policy

## Replica

`REP` **Count**(*number*) [ `IN` **SelectorName**(*string*) ]

> multiple

## ContainerBackupFactor

`CBF` **Count**(*number*)

> optional 

## Selector

`SELECT` **Count**(*number*) `IN` [ **Clause**("*SAME*", "*DISTINCT*") ]  **Attribute**(*string*) `FROM` **FilterName**(*string*, "\*") [ `AS` **SelectorName**(*string*) ]

> multiple, optional 

## Filter

`FILTER` **Condition**(*string*) [ `AS` **FilterName**(*string*) ]

> multiple, optional

#### Filter Condition


  * Compare Operation

    `EQ`, `NE`, `GT`, `GE`, `LT`, `LE` 

  * Logic Operation

    `OR`, `AND`

  * Compare Statement

    **Attribute**(*string*) `EQ`|`NE`|`GT`|`GE`|`TE`  **Value**(*string*)

  * Logic Statement

    **CompareStatement** `AND`|`OR` **CompareStatement**
    
So condition statement is as follows:

 **CompareStatement** [ `AND` **CompareStatement** ]... [ `OR` **CompareStatement** [ `AND` **CompareStatement** ]... ]...




