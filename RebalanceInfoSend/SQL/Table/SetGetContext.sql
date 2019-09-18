-----------------Sql Server 2008 upto 2014---------------------------------------------
-- Initial call just after creating a connection from LV adapter to identify the source.

-- 1.
Create Procedure SetContext (@contextname nvarchar(32)) as
DECLARE @context varbinary(128);
begin
SELECT @context = CONVERT(varbinary(128), @contextname);
SET CONTEXT_INFO @context;
end;


-- In trigger / SP , we will check the value. If it is set as 'LV Adapter', we will skip the rest of the code.
-- That way we could control the trigger process

-- 2. 
Create Procedure GetContext (@contextname nVarchar(128) output) as
begin
SELECT @contextname = CONVERT(nVarChar(128), CONTEXT_INFO());
end;

-------------------Sql Server 2008 upto 2014---------------------------------------------

-------------------Sql Server 2016------------------------------------------------------
-- Note for Sql-Server 2016 the context can be set using key/value pair.
 
-- 3.
Create Procedure SetContext (@contextname nvarchar(32)) as
DECLARE @context varbinary(128);
begin
EXEC sp_set_session_context 'SessionSource', @contextname; 
end;

--4. 
Create Procedure GetContext (@pcontext nVarchar(128) output) as
begin
SELECT @pcontext = CONVERT(nVarChar(128),  SESSION_CONTEXT(N'SessionSource') ); 
end;

-------------------Sql Server 2016------------------------------------------------------





/*  

------------------Testing from SSMS ( Sql Server 2014).---------------------------------

Step 0 : Compile above SP 1 and 2.

Step 1: Open a SSMS session.

Step 2: call 1st SP that sets the context by 'exec SetContext'

Step 3 : Call 2nd sp to check the context in the same session 

declare @showcontext varchar(128);
begin
  exec GetContext @showcontext output;
  print @showcontext;
end;

  Should Return 'LV Adapter'

Step 4 : Open another session and run following

declare @showcontext varchar(128);
begin
  exec GetContext @showcontext output;
  print @showcontext;
end;

  Should Return NULL

------------------Testing from SSMS ( Sql Server 2014).---------------------------------


------------------Testing from SSMS ( Sql Server 2016).---------------------------------
 
Step 0 : Compile above SP 3 and 4.

Step 1: Open a SSMS session.

Step 2: call 3rd SP that sets the context by 'exec SetContext'

Step 3 : Call 4th sp to check the context in the same session 

declare @showcontext varchar(128);
begin
  exec GetContext @showcontext output;
  print @showcontext;
end;

  Should Return 'LV Adapter'

Step 4 : Open another session and run following

declare @showcontext varchar(128);
begin
  exec GetContext @showcontext output;
  print @showcontext;
end;

  Should Return NULL


------------------Testing from SSMS ( Sql Server 2016).---------------------------------

*/

declare @a 
exec getcontext