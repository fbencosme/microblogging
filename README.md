**Como correr la app?**


### Pasos: ###

* Ejecutar el script llamado 

```
#!sql

DatabaseScript.sql.
```

(ubicado en el root path del proyecto)


* Tener soporte para ASP.NET 5.0 y MVC 6. (en visual studio o dnx)*
 
* Cambiar "SQLEXPRESS" si es necesario. *
  Ruta:
    
```
#!c#

Archivo: DbContext.cs 
linea  : 19
```

   
* Si tiene VS bien configuardo(punto 2), debe solo darle a ejecutar (F5).


*  Visite,
```
#!bash

 http://localhost:5000/
```



**Stack**.

1. ASP.NET 5.0 y MVC 6.
2. AngularJS
3. Libraries: 
   1. * lodash: https://lodash.com/
   1. * moment: http://momentjs.com/
   1. * angular-material: https://material.angularjs.org/