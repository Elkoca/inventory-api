$data = Invoke-RestMethod -Uri "https://dummyjson.com/products"

$articleNo = 1115212
foreach ($product in $data.products) {
    $body = [PSCustomObject]@{
        name = "$($articleNo) | $($product.title)"
        title = $product.title
        description = $product.description
    } | ConvertTo-Json

    $articleNo = $articleNo + 111
    Invoke-RestMethod -Uri "https://localhost:7156/api/Products" -method Post -Body $body -ContentType "application/json"
}