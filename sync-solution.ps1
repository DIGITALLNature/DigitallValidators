pac auth select --name DigitallValidators
if ($LASTEXITCODE -ne 0) {
    return;
}

pac solution sync --solution-folder ./solutions/dgt_validators --async