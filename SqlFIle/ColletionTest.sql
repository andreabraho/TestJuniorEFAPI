declare @VariabileMaiuscola char(1)
declare @VariabileMinuscola char(1)
declare @VariabileNonAccentata char(1)
declare @VariabileAccentata char(1)

set @VariabileMaiuscola ='A'
set @VariabileMinuscola ='a'
set @VariabileNonAccentata ='a'
set @VariabileAccentata ='�'

if @VariabileMaiuscola = @VariabileMinuscola
begin
print 'A ed a sono uguali'
end
else
begin
print 'A ed a non sono uguali'
end

if @VariabileNonAccentata = @VariabileAccentata 
begin
print 'a ed � sono uguali'
end
else
begin
print 'a ed � non sono uguali'
end