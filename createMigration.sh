#!/usr/bin/env bash

if [[ -z "$1" ]]; then
  echo "Usage:"
  echo "  $0 [migration-name]"
  exit 1
fi

dotnet ef migrations add $1 --project src/MyInvest

