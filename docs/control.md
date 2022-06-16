# Protocol Documentation
<a name="top"></a>

## Table of Contents

- [control/service.proto](#control/service.proto)
 - Services
    - [ControlService](#control.ControlService)
    
  - Messages
    - [DropObjectsRequest](#control.DropObjectsRequest)
    - [DropObjectsRequest.Body](#control.DropObjectsRequest.Body)
    - [DropObjectsResponse](#control.DropObjectsResponse)
    - [DropObjectsResponse.Body](#control.DropObjectsResponse.Body)
    - [DumpShardRequest](#control.DumpShardRequest)
    - [DumpShardRequest.Body](#control.DumpShardRequest.Body)
    - [DumpShardResponse](#control.DumpShardResponse)
    - [DumpShardResponse.Body](#control.DumpShardResponse.Body)
    - [HealthCheckRequest](#control.HealthCheckRequest)
    - [HealthCheckRequest.Body](#control.HealthCheckRequest.Body)
    - [HealthCheckResponse](#control.HealthCheckResponse)
    - [HealthCheckResponse.Body](#control.HealthCheckResponse.Body)
    - [ListShardsRequest](#control.ListShardsRequest)
    - [ListShardsRequest.Body](#control.ListShardsRequest.Body)
    - [ListShardsResponse](#control.ListShardsResponse)
    - [ListShardsResponse.Body](#control.ListShardsResponse.Body)
    - [NetmapSnapshotRequest](#control.NetmapSnapshotRequest)
    - [NetmapSnapshotRequest.Body](#control.NetmapSnapshotRequest.Body)
    - [NetmapSnapshotResponse](#control.NetmapSnapshotResponse)
    - [NetmapSnapshotResponse.Body](#control.NetmapSnapshotResponse.Body)
    - [RestoreShardRequest](#control.RestoreShardRequest)
    - [RestoreShardRequest.Body](#control.RestoreShardRequest.Body)
    - [RestoreShardResponse](#control.RestoreShardResponse)
    - [RestoreShardResponse.Body](#control.RestoreShardResponse.Body)
    - [SetNetmapStatusRequest](#control.SetNetmapStatusRequest)
    - [SetNetmapStatusRequest.Body](#control.SetNetmapStatusRequest.Body)
    - [SetNetmapStatusResponse](#control.SetNetmapStatusResponse)
    - [SetNetmapStatusResponse.Body](#control.SetNetmapStatusResponse.Body)
    - [SetShardModeRequest](#control.SetShardModeRequest)
    - [SetShardModeRequest.Body](#control.SetShardModeRequest.Body)
    - [SetShardModeResponse](#control.SetShardModeResponse)
    - [SetShardModeResponse.Body](#control.SetShardModeResponse.Body)
    

- [control/types.proto](#control/types.proto)

  - Messages
    - [Netmap](#control.Netmap)
    - [NodeInfo](#control.NodeInfo)
    - [NodeInfo.Attribute](#control.NodeInfo.Attribute)
    - [ShardInfo](#control.ShardInfo)
    - [Signature](#control.Signature)
    

- [Scalar Value Types](#scalar-value-types)



<a name="control/service.proto"></a>
<p align="right"><a href="#top">Top</a></p>

## control/service.proto




<a name="control.ControlService"></a>

### Service "control.ControlService"
`ControlService` provides an interface for internal work with the storage node.

```
rpc HealthCheck(HealthCheckRequest) returns (HealthCheckResponse);
rpc NetmapSnapshot(NetmapSnapshotRequest) returns (NetmapSnapshotResponse);
rpc SetNetmapStatus(SetNetmapStatusRequest) returns (SetNetmapStatusResponse);
rpc DropObjects(DropObjectsRequest) returns (DropObjectsResponse);
rpc ListShards(ListShardsRequest) returns (ListShardsResponse);
rpc SetShardMode(SetShardModeRequest) returns (SetShardModeResponse);
rpc DumpShard(DumpShardRequest) returns (DumpShardResponse);
rpc RestoreShard(RestoreShardRequest) returns (RestoreShardResponse);

```

#### Method HealthCheck

Performs health check of the storage node.

| Name | Input | Output |
| ---- | ----- | ------ |
| HealthCheck | [HealthCheckRequest](#control.HealthCheckRequest) | [HealthCheckResponse](#control.HealthCheckResponse) |
#### Method NetmapSnapshot

Returns network map snapshot of the current NeoFS epoch.

| Name | Input | Output |
| ---- | ----- | ------ |
| NetmapSnapshot | [NetmapSnapshotRequest](#control.NetmapSnapshotRequest) | [NetmapSnapshotResponse](#control.NetmapSnapshotResponse) |
#### Method SetNetmapStatus

Sets status of the storage node in NeoFS network map.

| Name | Input | Output |
| ---- | ----- | ------ |
| SetNetmapStatus | [SetNetmapStatusRequest](#control.SetNetmapStatusRequest) | [SetNetmapStatusResponse](#control.SetNetmapStatusResponse) |
#### Method DropObjects

Mark objects to be removed from node's local object storage.

| Name | Input | Output |
| ---- | ----- | ------ |
| DropObjects | [DropObjectsRequest](#control.DropObjectsRequest) | [DropObjectsResponse](#control.DropObjectsResponse) |
#### Method ListShards

Returns list that contains information about all shards of a node.

| Name | Input | Output |
| ---- | ----- | ------ |
| ListShards | [ListShardsRequest](#control.ListShardsRequest) | [ListShardsResponse](#control.ListShardsResponse) |
#### Method SetShardMode

Sets mode of the shard.

| Name | Input | Output |
| ---- | ----- | ------ |
| SetShardMode | [SetShardModeRequest](#control.SetShardModeRequest) | [SetShardModeResponse](#control.SetShardModeResponse) |
#### Method DumpShard

Dump objects from the shard.

| Name | Input | Output |
| ---- | ----- | ------ |
| DumpShard | [DumpShardRequest](#control.DumpShardRequest) | [DumpShardResponse](#control.DumpShardResponse) |
#### Method RestoreShard

Restore objects from dump.

| Name | Input | Output |
| ---- | ----- | ------ |
| RestoreShard | [RestoreShardRequest](#control.RestoreShardRequest) | [RestoreShardResponse](#control.RestoreShardResponse) |
 <!-- end services -->


<a name="control.DropObjectsRequest"></a>

### Message DropObjectsRequest
Request to drop the objects.


| Field | Type | Label | Description |
| ----- | ---- | ----- | ----------- |
| body | [DropObjectsRequest.Body](#control.DropObjectsRequest.Body) |  | Body of the request message. |
| signature | [Signature](#control.Signature) |  | Body signature. |


<a name="control.DropObjectsRequest.Body"></a>

### Message DropObjectsRequest.Body
Request body structure.


| Field | Type | Label | Description |
| ----- | ---- | ----- | ----------- |
| address_list | [bytes](#bytes) | repeated | List of object addresses to be removed. in NeoFS API binary format. |


<a name="control.DropObjectsResponse"></a>

### Message DropObjectsResponse
Response to request to drop the objects.


| Field | Type | Label | Description |
| ----- | ---- | ----- | ----------- |
| body | [DropObjectsResponse.Body](#control.DropObjectsResponse.Body) |  | Body of the response message. |
| signature | [Signature](#control.Signature) |  | Body signature. |


<a name="control.DropObjectsResponse.Body"></a>

### Message DropObjectsResponse.Body
Response body structure.



<a name="control.DumpShardRequest"></a>

### Message DumpShardRequest
DumpShard request.


| Field | Type | Label | Description |
| ----- | ---- | ----- | ----------- |
| body | [DumpShardRequest.Body](#control.DumpShardRequest.Body) |  | Body of dump shard request message. |
| signature | [Signature](#control.Signature) |  | Body signature. |


<a name="control.DumpShardRequest.Body"></a>

### Message DumpShardRequest.Body
Request body structure.


| Field | Type | Label | Description |
| ----- | ---- | ----- | ----------- |
| shard_ID | [bytes](#bytes) |  | ID of the shard. |
| filepath | [string](#string) |  | Path to the output. |
| ignore_errors | [bool](#bool) |  | Flag indicating whether object read errors should be ignored. |


<a name="control.DumpShardResponse"></a>

### Message DumpShardResponse
DumpShard response.


| Field | Type | Label | Description |
| ----- | ---- | ----- | ----------- |
| body | [DumpShardResponse.Body](#control.DumpShardResponse.Body) |  | Body of dump shard response message. |
| signature | [Signature](#control.Signature) |  | Body signature. |


<a name="control.DumpShardResponse.Body"></a>

### Message DumpShardResponse.Body
Response body structure.



<a name="control.HealthCheckRequest"></a>

### Message HealthCheckRequest
Health check request.


| Field | Type | Label | Description |
| ----- | ---- | ----- | ----------- |
| body | [HealthCheckRequest.Body](#control.HealthCheckRequest.Body) |  | Body of health check request message. |
| signature | [Signature](#control.Signature) |  | Body signature. |


<a name="control.HealthCheckRequest.Body"></a>

### Message HealthCheckRequest.Body
Health check request body.



<a name="control.HealthCheckResponse"></a>

### Message HealthCheckResponse
Health check request.


| Field | Type | Label | Description |
| ----- | ---- | ----- | ----------- |
| body | [HealthCheckResponse.Body](#control.HealthCheckResponse.Body) |  | Body of health check response message. |
| signature | [Signature](#control.Signature) |  | Body signature. |


<a name="control.HealthCheckResponse.Body"></a>

### Message HealthCheckResponse.Body
Health check response body


| Field | Type | Label | Description |
| ----- | ---- | ----- | ----------- |
| netmap_status | [NetmapStatus](#control.NetmapStatus) |  | Status of the storage node in NeoFS network map. |
| health_status | [HealthStatus](#control.HealthStatus) |  | Health status of storage node application. |


<a name="control.ListShardsRequest"></a>

### Message ListShardsRequest
Request to list all shards of the node.


| Field | Type | Label | Description |
| ----- | ---- | ----- | ----------- |
| body | [ListShardsRequest.Body](#control.ListShardsRequest.Body) |  | Body of the request message. |
| signature | [Signature](#control.Signature) |  | Body signature. |


<a name="control.ListShardsRequest.Body"></a>

### Message ListShardsRequest.Body
Request body structure.



<a name="control.ListShardsResponse"></a>

### Message ListShardsResponse
ListShards response.


| Field | Type | Label | Description |
| ----- | ---- | ----- | ----------- |
| body | [ListShardsResponse.Body](#control.ListShardsResponse.Body) |  | Body of the response message. |
| signature | [Signature](#control.Signature) |  | Body signature. |


<a name="control.ListShardsResponse.Body"></a>

### Message ListShardsResponse.Body
Response body structure.


| Field | Type | Label | Description |
| ----- | ---- | ----- | ----------- |
| shards | [ShardInfo](#control.ShardInfo) | repeated | List of the node's shards. |


<a name="control.NetmapSnapshotRequest"></a>

### Message NetmapSnapshotRequest
Get netmap snapshot request.


| Field | Type | Label | Description |
| ----- | ---- | ----- | ----------- |
| body | [NetmapSnapshotRequest.Body](#control.NetmapSnapshotRequest.Body) |  | Body of get netmap snapshot request message. |
| signature | [Signature](#control.Signature) |  | Body signature. |


<a name="control.NetmapSnapshotRequest.Body"></a>

### Message NetmapSnapshotRequest.Body
Get netmap snapshot request body.



<a name="control.NetmapSnapshotResponse"></a>

### Message NetmapSnapshotResponse
Get netmap snapshot request.


| Field | Type | Label | Description |
| ----- | ---- | ----- | ----------- |
| body | [NetmapSnapshotResponse.Body](#control.NetmapSnapshotResponse.Body) |  | Body of get netmap snapshot response message. |
| signature | [Signature](#control.Signature) |  | Body signature. |


<a name="control.NetmapSnapshotResponse.Body"></a>

### Message NetmapSnapshotResponse.Body
Get netmap snapshot response body


| Field | Type | Label | Description |
| ----- | ---- | ----- | ----------- |
| netmap | [Netmap](#control.Netmap) |  | Structure of the requested network map. |


<a name="control.RestoreShardRequest"></a>

### Message RestoreShardRequest
RestoreShard request.


| Field | Type | Label | Description |
| ----- | ---- | ----- | ----------- |
| body | [RestoreShardRequest.Body](#control.RestoreShardRequest.Body) |  | Body of restore shard request message. |
| signature | [Signature](#control.Signature) |  | Body signature. |


<a name="control.RestoreShardRequest.Body"></a>

### Message RestoreShardRequest.Body
Request body structure.


| Field | Type | Label | Description |
| ----- | ---- | ----- | ----------- |
| shard_ID | [bytes](#bytes) |  | ID of the shard. |
| filepath | [string](#string) |  | Path to the output. |
| ignore_errors | [bool](#bool) |  | Flag indicating whether object read errors should be ignored. |


<a name="control.RestoreShardResponse"></a>

### Message RestoreShardResponse
RestoreShard response.


| Field | Type | Label | Description |
| ----- | ---- | ----- | ----------- |
| body | [RestoreShardResponse.Body](#control.RestoreShardResponse.Body) |  | Body of restore shard response message. |
| signature | [Signature](#control.Signature) |  | Body signature. |


<a name="control.RestoreShardResponse.Body"></a>

### Message RestoreShardResponse.Body
Response body structure.



<a name="control.SetNetmapStatusRequest"></a>

### Message SetNetmapStatusRequest
Set netmap status request.


| Field | Type | Label | Description |
| ----- | ---- | ----- | ----------- |
| body | [SetNetmapStatusRequest.Body](#control.SetNetmapStatusRequest.Body) |  | Body of set netmap status request message. |
| signature | [Signature](#control.Signature) |  | Body signature. |


<a name="control.SetNetmapStatusRequest.Body"></a>

### Message SetNetmapStatusRequest.Body
Set netmap status request body.


| Field | Type | Label | Description |
| ----- | ---- | ----- | ----------- |
| status | [NetmapStatus](#control.NetmapStatus) |  | New storage node status in NeoFS network map. |


<a name="control.SetNetmapStatusResponse"></a>

### Message SetNetmapStatusResponse
Set netmap status response.


| Field | Type | Label | Description |
| ----- | ---- | ----- | ----------- |
| body | [SetNetmapStatusResponse.Body](#control.SetNetmapStatusResponse.Body) |  | Body of set netmap status response message. |
| signature | [Signature](#control.Signature) |  | Body signature. |


<a name="control.SetNetmapStatusResponse.Body"></a>

### Message SetNetmapStatusResponse.Body
Set netmap status response body



<a name="control.SetShardModeRequest"></a>

### Message SetShardModeRequest
Request to set mode of the shard.


| Field | Type | Label | Description |
| ----- | ---- | ----- | ----------- |
| body | [SetShardModeRequest.Body](#control.SetShardModeRequest.Body) |  | Body of set shard mode request message. |
| signature | [Signature](#control.Signature) |  | Body signature. |


<a name="control.SetShardModeRequest.Body"></a>

### Message SetShardModeRequest.Body
Request body structure.


| Field | Type | Label | Description |
| ----- | ---- | ----- | ----------- |
| shard_ID | [bytes](#bytes) |  | ID of the shard. |
| mode | [ShardMode](#control.ShardMode) |  | Mode that requested to be set. |
| resetErrorCounter | [bool](#bool) |  | Flag signifying whether error counter should be set to 0. |


<a name="control.SetShardModeResponse"></a>

### Message SetShardModeResponse
SetShardMode response.


| Field | Type | Label | Description |
| ----- | ---- | ----- | ----------- |
| body | [SetShardModeResponse.Body](#control.SetShardModeResponse.Body) |  | Body of set shard mode response message. |
| signature | [Signature](#control.Signature) |  | Body signature. |


<a name="control.SetShardModeResponse.Body"></a>

### Message SetShardModeResponse.Body
Response body structure.


 <!-- end messages -->

 <!-- end enums -->



<a name="control/types.proto"></a>
<p align="right"><a href="#top">Top</a></p>

## control/types.proto


 <!-- end services -->


<a name="control.Netmap"></a>

### Message Netmap
Network map structure.


| Field | Type | Label | Description |
| ----- | ---- | ----- | ----------- |
| epoch | [uint64](#uint64) |  | Network map revision number. |
| nodes | [NodeInfo](#control.NodeInfo) | repeated | Nodes presented in network. |


<a name="control.NodeInfo"></a>

### Message NodeInfo
NeoFS node description.


| Field | Type | Label | Description |
| ----- | ---- | ----- | ----------- |
| public_key | [bytes](#bytes) |  | Public key of the NeoFS node in a binary format. |
| addresses | [string](#string) | repeated | Ways to connect to a node. |
| attributes | [NodeInfo.Attribute](#control.NodeInfo.Attribute) | repeated | Carries list of the NeoFS node attributes in a key-value form. Key name must be a node-unique valid UTF-8 string. Value can't be empty. NodeInfo structures with duplicated attribute names or attributes with empty values will be considered invalid. |
| state | [NetmapStatus](#control.NetmapStatus) |  | Carries state of the NeoFS node. |


<a name="control.NodeInfo.Attribute"></a>

### Message NodeInfo.Attribute
Administrator-defined Attributes of the NeoFS Storage Node.

`Attribute` is a Key-Value metadata pair. Key name must be a valid UTF-8
string. Value can't be empty.

Node's attributes are mostly used during Storage Policy evaluation to
calculate object's placement and find a set of nodes satisfying policy
requirements. There are some "well-known" node attributes common to all the
Storage Nodes in the network and used implicitly with default values if not
explicitly set:

* Capacity \
  Total available disk space in Gigabytes.
* Price \
  Price in GAS tokens for storing one GB of data during one Epoch. In node
  attributes it's a string presenting floating point number with comma or
  point delimiter for decimal part. In the Network Map it will be saved as
  64-bit unsigned integer representing number of minimal token fractions.
* Subnet \
  String ID of Node's storage subnet. There can be only one subnet served
  by the Storage Node.
* Locode \
  Node's geographic location in
  [UN/LOCODE](https://www.unece.org/cefact/codesfortrade/codes_index.html)
  format approximated to the nearest point defined in standard.
* Country \
  Country code in
  [ISO 3166-1_alpha-2](https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2)
  format. Calculated automatically from `Locode` attribute
* Region \
  Country's administative subdivision where node is located. Calculated
  automatically from `Locode` attribute based on `SubDiv` field. Presented
  in [ISO 3166-2](https://en.wikipedia.org/wiki/ISO_3166-2) format.
* City \
  City, town, village or rural area name where node is located written
  without diacritics . Calculated automatically from `Locode` attribute.

For detailed description of each well-known attribute please see the
corresponding section in NeoFS Technical specification.


| Field | Type | Label | Description |
| ----- | ---- | ----- | ----------- |
| key | [string](#string) |  | Key of the node attribute. |
| value | [string](#string) |  | Value of the node attribute. |
| parents | [string](#string) | repeated | Parent keys, if any. For example for `City` it could be `Region` and `Country`. |


<a name="control.ShardInfo"></a>

### Message ShardInfo
Shard description.


| Field | Type | Label | Description |
| ----- | ---- | ----- | ----------- |
| shard_ID | [bytes](#bytes) |  | ID of the shard. |
| metabase_path | [string](#string) |  | Path to shard's metabase. |
| blobstor_path | [string](#string) |  | Path to shard's blobstor. |
| writecache_path | [string](#string) |  | Path to shard's write-cache, empty if disabled. |
| mode | [ShardMode](#control.ShardMode) |  | Work mode of the shard. |
| errorCount | [uint32](#uint32) |  | Amount of errors occured. |


<a name="control.Signature"></a>

### Message Signature
Signature of some message.


| Field | Type | Label | Description |
| ----- | ---- | ----- | ----------- |
| key | [bytes](#bytes) |  | Public key used for signing. |
| sign | [bytes](#bytes) |  | Binary signature. |

 <!-- end messages -->


<a name="control.HealthStatus"></a>

### HealthStatus
Health status of the storage node application.

| Name | Number | Description |
| ---- | ------ | ----------- |
| HEALTH_STATUS_UNDEFINED | 0 | Undefined status, default value. |
| STARTING | 1 | Storage node application is starting. |
| READY | 2 | Storage node application is started and serves all services. |
| SHUTTING_DOWN | 3 | Storage node application is shutting down. |



<a name="control.NetmapStatus"></a>

### NetmapStatus
Status of the storage node in the NeoFS network map.

| Name | Number | Description |
| ---- | ------ | ----------- |
| STATUS_UNDEFINED | 0 | Undefined status, default value. |
| ONLINE | 1 | Node is online. |
| OFFLINE | 2 | Node is offline. |
| MAINTENANCE | 3 | Node is maintained by the owner. |



<a name="control.ShardMode"></a>

### ShardMode
Work mode of the shard.

| Name | Number | Description |
| ---- | ------ | ----------- |
| SHARD_MODE_UNDEFINED | 0 | Undefined mode, default value. |
| READ_WRITE | 1 | Read-write. |
| READ_ONLY | 2 | Read-only. |
| DEGRADED | 3 | Degraded. |


 <!-- end enums -->



## Scalar Value Types

| .proto Type | Notes | C++ Type | Java Type | Python Type |
| ----------- | ----- | -------- | --------- | ----------- |
| <a name="double" /> double |  | double | double | float |
| <a name="float" /> float |  | float | float | float |
| <a name="int32" /> int32 | Uses variable-length encoding. Inefficient for encoding negative numbers – if your field is likely to have negative values, use sint32 instead. | int32 | int | int |
| <a name="int64" /> int64 | Uses variable-length encoding. Inefficient for encoding negative numbers – if your field is likely to have negative values, use sint64 instead. | int64 | long | int/long |
| <a name="uint32" /> uint32 | Uses variable-length encoding. | uint32 | int | int/long |
| <a name="uint64" /> uint64 | Uses variable-length encoding. | uint64 | long | int/long |
| <a name="sint32" /> sint32 | Uses variable-length encoding. Signed int value. These more efficiently encode negative numbers than regular int32s. | int32 | int | int |
| <a name="sint64" /> sint64 | Uses variable-length encoding. Signed int value. These more efficiently encode negative numbers than regular int64s. | int64 | long | int/long |
| <a name="fixed32" /> fixed32 | Always four bytes. More efficient than uint32 if values are often greater than 2^28. | uint32 | int | int |
| <a name="fixed64" /> fixed64 | Always eight bytes. More efficient than uint64 if values are often greater than 2^56. | uint64 | long | int/long |
| <a name="sfixed32" /> sfixed32 | Always four bytes. | int32 | int | int |
| <a name="sfixed64" /> sfixed64 | Always eight bytes. | int64 | long | int/long |
| <a name="bool" /> bool |  | bool | boolean | boolean |
| <a name="string" /> string | A string must always contain UTF-8 encoded or 7-bit ASCII text. | string | String | str/unicode |
| <a name="bytes" /> bytes | May contain any arbitrary sequence of bytes. | string | ByteString | str |

