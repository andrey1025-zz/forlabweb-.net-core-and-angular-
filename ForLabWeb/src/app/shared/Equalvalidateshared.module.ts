
import { NgModule } from '@angular/core';
import { EqualValidator } from './equal-validator.directive';
@NgModule({
 
    declarations: [
    
      EqualValidator
     
   
    
    
   
  
    ],
    imports: [ // import Angular's modules
   
    ],
    exports: [
        EqualValidator
    ],
    providers: [ // expose our Services and Providers into Angular's dependency injection
      // ENV_PROVIDERS,
  
    ]
  })
  export class SharedequalModule {
  
  
  
  }