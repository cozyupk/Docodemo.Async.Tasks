#!/bin/sh

DIR=$( cd -- "$( dirname -- "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )
SOLDIR="${DIR}/../../"
echo "SOLDIR: SOLDIR: ${SOLDIR}"

for proj in $(find . -name '*.csproj'); do
  name=$(basename "${proj}" .csproj)
  dotnet test "${proj}" --no-build \
    -p:CollectCoverage=true \
    -p:CoverletOutput="${SOLDIR}/coverage/${name}.cobertura.xml" \
    -p:CoverletOutputFormat=cobertura
done

files=""

cd "${SOLDIR}/coverage/" && \
  for f in *.cobertura.xml; do 
    if [ -n "${files}" ]; then
      files="${files};"
    fi
    files="${files}$f"
  done

reportgenerator -reports:$files -targetdir:Documents/utest_coverages/ -reporttypes:Html
