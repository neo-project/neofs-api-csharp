# Container Placement Policy

# Format

## Replica

```
REP $Count(number) [in $SelectorName(string)]
```

## ContainerBackupFactor

```
CBF $Count(number)
```

## Selector

```
SELECT $Count(number) IN [$Clause("SAME", "DISTINCT")] $Attribute(string) FROM $FilterName(string, "*") [AS $SelectorName(string)]
```

## Filter

```
FILTER $Condition(string) [AS $FilterName(string)]
```

### Filter Condition


  * Compare Operation

    `EQ`, `NE`, `GT`, `GE`, `LT`, `LE` 

  * Logic Operation

    `OR`, `AND`

  * Compare Statement

    ```
    $Attribute(string) EQ|NE|GT|GETE  $value(string)
    ```

  * Logic Statement

    ```
    $CompareStatement AND|OR $CompareStatement
    ```
    
So condition statement is as follows:

```
 $CompareStatement [AND $CompareStatement ]... [OR $CompareStatement [AND $CompareStatement ]... ]...
```



