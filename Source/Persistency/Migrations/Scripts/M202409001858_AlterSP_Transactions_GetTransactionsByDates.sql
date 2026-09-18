SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[Transactions_GetTransactionsByDates]
(
	@DateFrom DateTime,
	@DateTo DateTime,
	@PageSize integer,
	@PageNumber integer,
	@ShopID NVARCHAR(50),
    @CustomerFirstName NVARCHAR(200),
	@CustomerLastName NVARCHAR(200),
    @ShoppingCartID NVARCHAR(50),
    @CreditCardName NVARCHAR(50)
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @sql nvarchar(max);

	SET @sql = N'
		SELECT 
		[Id],
		[WsPayOrderId],
		[UniqueTransactionNumber],
		[Signature],
		[STAN],
		[ApprovalCode],
		[ErrorMessage],
		[ShopID],
		[ShoppingCartID],
		[Amount],
		[CurrencyCode],
		[Success],
		[Authorized],
		[Completed],
		[Voided],
		[Refunded],
		[PaymentPlan],
		[Partner],
		[OnSite],
		[CreditCardName],
		[CreditCardNumber],
		[ECI],
		[CustomerFirstName],
		[CustomerLastName],
		[CustomerAddress],
		[CustomerCity],
		[CustomerCountry],
		[CustomerPhone],
		[CustomerZIP],
		[CustomerEmail],
		[TransactionDateTime],
		[Token],
		[TokenNumber],
		[ExpirationDate]
	FROM 
		[dbo].[Transaction]
	WHERE 
		TransactionDateTime BETWEEN @DateFrom AND @DateTo
	';
	
	IF @ShopID IS NOT NULL
	BEGIN
		SET @sql = @sql + N' AND ShopID IN (SELECT *
					FROM dbo.ParameterListToTable(@ShopID, ''|''))';
	END

	IF @ShoppingCartID IS NOT NULL
	BEGIN
		SET @sql = @sql + N' AND ShoppingCartID = @ShoppingCartID';
	END

	IF @CreditCardName IS NOT NULL
	BEGIN
		SET @sql = @sql + N' AND CreditCardName = @CreditCardName';
	END

	IF @CustomerFirstName IS NOT NULL
	BEGIN
		SET @sql = @sql + N' AND CustomerFirstName = @CustomerFirstName';
	END

	IF @CustomerLastName IS NOT NULL
	BEGIN
		SET @sql = @sql + N' AND CustomerLastName = @CustomerLastName';
	END

	SET @sql = @sql + N' ORDER BY TransactionDateTime DESC
    OFFSET @PageSize * (@PageNumber - 1) ROWS
    FETCH NEXT @PageSize ROWS ONLY';
	
	print @sql;
	EXEC sp_executesql @sql, 
        N'@DateFrom DateTime, @DateTo DateTime, @ShopID NVARCHAR(50), @CustomerFirstName NVARCHAR(200), @CustomerLastName NVARCHAR(200), @ShoppingCartID NVARCHAR(50), @CreditCardName NVARCHAR(50), @PageSize int, @PageNumber int', 
        @DateFrom, @DateTo, @ShopID, @CustomerFirstName, @CustomerLastName, @ShoppingCartID, @CreditCardName, @PageSize, @PageNumber;
END
GO