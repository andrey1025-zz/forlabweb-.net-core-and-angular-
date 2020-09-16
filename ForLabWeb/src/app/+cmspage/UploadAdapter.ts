
// export class UploadAdapter {
//     private loader;
//     constructor(loader: any) {
//       this.loader = loader;
//       this.readThis(loader.file);
    
//     }
  
//     public upload(): Promise<any> {
//       //"data:image/png;base64,"+ btoa(binaryString) 
//       return this.readThis(this.loader.file);
//     }
  
//     readThis(file: File): Promise<any> {
//       console.log(file)
//       let imagePromise: Promise<any> = new Promise((resolve, reject) => {
//         var myReader: FileReader = new FileReader();
//         myReader.onloadend = (e) => {
//           let image = myReader.result;
//           console.log(image);
//           return { default: "data:image/png;base64," + image };
//         }
//         myReader.readAsDataURL(file);
//       });
//       return imagePromise;
//     }
  
//   }
export class  UploadAdapter {
    loader;  // your adapter communicates to CKEditor through this
    url;
    constructor(loader, url) {
      this.loader = loader;   
      this.url = url;
      console.log('Upload Adapter Constructor', this.loader, this.url);
    }
  
    upload() {

        return this.loader.file
        .then( file => new Promise( ( resolve, reject ) => {
              var myReader= new FileReader();
              myReader.onloadend = (e) => {
                 resolve({ default: myReader.result });
              }

              myReader.readAsDataURL(file);
        } ) );
    //   return new Promise((resolve, reject) => {
    //     console.log('UploadAdapter upload called', this.loader, this.url);
    //     console.log('the file we got was', this.loader.file);
    //    resolve({ default: image }); 
    //   });
    }
  
    abort() {
      console.log('UploadAdapter abort');
    }
}
