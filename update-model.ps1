dgtp profile select DigitallValidators
if ($LASTEXITCODE -ne 0) {
    return;
}

dgtp codegeneration ./src/dgt.solutions.Model --folder Dataverse --config ./src/dgt.solutions.Model/model.json