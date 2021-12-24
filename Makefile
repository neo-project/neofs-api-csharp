SHELL := /bin/bash

PROTO_VERSION=v2.11.0
PROTO_URL=https://github.com/nspcc-dev/neofs-api/archive/$(PROTO_VERSION).tar.gz
CONTROL_SERVICE_PROTO_UTL=https://raw.githubusercontent.com/nspcc-dev/neofs-node/master/pkg/services/control

GOGO_VERSION=v1.3.1
GOGO_URL=https://github.com/gogo/protobuf/archive/$(GOGO_VERSION).tar.gz

DOC_GEN_URL=https://github.com/pseudomuto/protoc-gen-doc/releases/download/v1.4.1/protoc-gen-doc-1.4.1.$(shell uname -s)-amd64.go1.15.2.tar.gz

B=\033[0;1m
G=\033[0;92m
R=\033[0m

os_type=unknown
os_build=x86

ifeq ($(shell uname -s),Darwin)
	os_type=macosx
endif
ifeq ($(shell uname -s),Linux)
	os_type=linux
endif
ifeq ($(shell uname -m),x86_64)
	os_build=x64
endif

PROTO_TOOLS_PATH=${HOME}/.nuget/packages/grpc.tools
PROTO_TOOLS_VERSION=$(shell ls $(PROTO_TOOLS_PATH) | sort -V | tail -n1)
PROTO_TOOLS_BIN=$(PROTO_TOOLS_PATH)/$(PROTO_TOOLS_VERSION)/tools/$(os_type)_$(os_build)/

.PHONY: deps docgen

# Dependencies
deps:
	@printf "${B}${G}⇒ Cleanup old files ${R}\n"
	@rm -rf vendor/proto
	@rm -rf vendor/github.com/gogo/protobuf
	@rm -rf vendor/github.com/pseudomuto/protoc-gen-doc

	@find src/Neo.FileStorage.API -type f -name '*.proto' -not -name '*_test.proto' -exec rm {} \;

	@printf "${B}${G}⇒ Install Proto-dependencies ${R}\n"
	@mkdir -p vendor/proto
	@mkdir -p vendor/github.com/gogo/protobuf
	@mkdir -p vendor/github.com/pseudomuto/protoc-gen-doc
	@curl -sL -o vendor/proto.tar.gz $(PROTO_URL)
	@tar -xzf vendor/proto.tar.gz --strip-components 1 -C vendor/proto
	@curl -sL -o vendor/gogo.tar.gz $(GOGO_URL)
	@tar -xzf vendor/gogo.tar.gz --strip-components 1 -C vendor/github.com/gogo/protobuf
	@curl -sL -o vendor/protoc-gen-doc.tar.gz $(DOC_GEN_URL)
	@tar -xzf vendor/protoc-gen-doc.tar.gz --strip-components 1 -C vendor/github.com/pseudomuto/protoc-gen-doc

	@printf "${B}${G}⇒ NeoFS Proto files ${R}\n"
	@for f in `find vendor/proto -type f -name '*.proto' -exec dirname {} \; | sort -u `; do \
		mkdir -p src/Neo.FileStorage.API/$$(basename $$f); \
		cp $$f/*.proto src/Neo.FileStorage.API/$$(basename $$f)/; \
	done

	@printf "NeoFS Node ControlService Proto files\n"
	@printf "Downloading proto files...\n"
	@curl -sL -o src/Neo.FileStorage.API/control/service.proto $(CONTROL_SERVICE_PROTO_UTL)/service.proto
	@curl -sL -o src/Neo.FileStorage.API/control/types.proto $(CONTROL_SERVICE_PROTO_UTL)/types.proto
	@printf "Replacing import path...\n"
	@sed -i "" "s/pkg\/services\/control\/types.proto/control\/types.proto/g" src/Neo.FileStorage.API/control/service.proto
	@printf "Inserting csharp namespace...\n"
	@sed -i '' '/go_package/ a\
	option csharp_namespace = "Neo.FileStorage.API.Control\";\
	' src/Neo.FileStorage.API/control/service.proto
	@sed -i '' '/go_package/ a\
	option csharp_namespace = "Neo.FileStorage.API.Control\";\
	' src/Neo.FileStorage.API/control/types.proto

	@printf "${B}${G}⇒ Cleanup ${R}\n"
	@rm -rf vendor/proto
	@rm -rf vendor/gogo.tar.gz
	@rm -rf vendor/proto.tar.gz

# Regenerate documentation for protot files:
docgen: deps
	@mkdir -p ./docs
	@for f in `find src/Neo.FileStorage.API -type f -name '*.proto' -exec dirname {} \; | sort -u `; do \
		printf "${B}${G}⇒ Documentation for $$(basename $$f) ${R}\n"; \
		protoc \
			--plugin=protoc-gen-doc=./vendor/github.com/pseudomuto/protoc-gen-doc/protoc-gen-doc \
			--doc_opt=.github/markdown.tmpl,$${f}.md \
			--proto_path=src/Neo.FileStorage.API:vendor:/usr/local/include \
			--doc_out=docs/ $${f}/*.proto; \
	done
