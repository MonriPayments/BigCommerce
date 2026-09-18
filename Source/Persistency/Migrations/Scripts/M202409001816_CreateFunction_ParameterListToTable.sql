SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[ParameterListToTable]
                 (@ParameterList      nvarchar(max), @delimiter nchar(1) = N',')
      RETURNS @tbl TABLE (Parameter nvarchar(4000)) AS

BEGIN
   DECLARE @pos      int,
           @textpos  int,
           @chunklen smallint,
           @tmpstr   nvarchar(4000),
           @leftover nvarchar(4000),
           @tmpval   nvarchar(4000)

   SET @textpos = 1
   SET @leftover = ''
   WHILE @textpos <= datalength(@ParameterList) / 2
   BEGIN
      SET @chunklen = 4000 - datalength(@leftover) / 2
      SET @tmpstr = @leftover + substring(@ParameterList, @textpos, @chunklen)
      SET @textpos = @textpos + @chunklen

      SET @pos = charindex(@delimiter, @tmpstr)

      WHILE @pos > 0
      BEGIN
         SET @tmpval = ltrim(rtrim(left(@tmpstr, @pos - 1)))
         INSERT @tbl (Parameter) VALUES(@tmpval)
         SET @tmpstr = substring(@tmpstr, @pos + 1, len(@tmpstr))
         SET @pos = charindex(@delimiter, @tmpstr)
      END

      SET @leftover = @tmpstr
   END

   INSERT @tbl(Parameter)
       VALUES (ltrim(rtrim(@leftover)))
   RETURN
END
GO

