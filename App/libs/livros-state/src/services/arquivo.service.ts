import { Injectable, OnInit } from '@angular/core';
import { FileUploader, FileItem, ParsedResponseHeaders, FileLikeObject, FileUploaderOptions } from 'ng2-file-upload';
import { ToastCoreService } from '@myorg/core';

@Injectable()
export class ArquivoService implements OnInit {      
   public uploader:FileUploader = new FileUploader({url:''});   
   public urlArquivo : string = '';
   public errorMessage: string= '';
   public successResponse : string = '';
   public allowedMimeType = ['image/png', 'image/jpg', 'image/jpeg', 'image/gif' ];
   public maxFileSize = 4 * 1024 * 1024;
   public isHTML5 = true;
   public isEnviado = false;   
   public autoUpload = true;
   
	constructor(/*private _configuration: ConfigurationService*/) 
	{      
		//this.urlArquivo = 'http://localhost:50000/Imagem/Upload';
		this.urlArquivo = 'http://localhost:50000/Livro/Upload';

		//this.initUploader();
		this.uploader = new FileUploader(
		{
			url: this.urlArquivo,
			isHTML5: this.isHTML5,
			autoUpload: this.autoUpload,
			itemAlias: "uploadedFile",
			allowedMimeType: this.allowedMimeType,
			//headers: [{name:'Accept', value:'application/json'}, {name:'Authorization', value:''}],
			headers: [{name:'Accept', value:'application/json'}],
			//headers: [{name:'Accept', value:'application/json'}, {name:'Content-Type:', value:'multipart/form-data'}],
			maxFileSize: this.maxFileSize
		});
	
		this.uploader.onAfterAddingFile = (file)=> { file.withCredentials = false; };
		this.uploader.onErrorItem = (item, response, status, headers) => this.onErrorItem(item, response, status, headers);
		this.uploader.onSuccessItem = (item, response, status, headers) => this.onSuccessItem(item, response, status, headers);
		this.uploader.onWhenAddingFileFailed = (item, filter, options) => this.onWhenAddingFileFailedService(item, filter, options);
		
	}

	ngOnInit() 
	{             
		/*this.uploader.onAfterAddingFile = (file)=> { file.withCredentials = false; };
		this.uploader.onErrorItem = (item, response, status, headers) => this.onErrorItem(item, response, status, headers);
		this.uploader.onSuccessItem = (item, response, status, headers) => this.onSuccessItem(item, response, status, headers);
		this.uploader.onWhenAddingFileFailed = (item, filter, options) => this.onWhenAddingFileFailedService(item, filter, options);
		this.uploader.onAfterAddingFile = (file)=> { file.withCredentials = false; };*/
	}

	public onFileSelected (event) 
	{    		
		var arquivos = event.srcElement.files as Array<File>;
		if(arquivos && arquivos.length>0) 
		{
			this.uploader.uploadAll();			
			return;
		}
		this.onLimparQueue(); 
	}

	public onFileUpload() 
	{    		
		this.uploader.uploadAll();
		return;
	}

	public onLimparQueue() 
	{
		this.uploader.clearQueue();
		this.successResponse =null;
		this.isEnviado =false;
	}

   public onSuccessItem(item: FileItem, response: string, status: number, headers: ParsedResponseHeaders): any {
		this.successResponse = response;
      this.errorMessage = null;
		this.isEnviado = true;
	  this.uploader.clearQueue();
   }

   public onErrorItem(item: FileItem, response: string, status: number, headers: ParsedResponseHeaders): any {      
      this.errorMessage = response;      
	  //this.commonService.toastr.error(this.errorMessage);
	  this.onLimparQueue();
   }

   public onWhenAddingFileFailedService(item: FileLikeObject, filter: any, options: any)  
   {
		switch (filter.name) 
		{
			case 'fileSize':
				this.errorMessage = `Tamanho máximo do arquivo é 4MB`;
			break;
			
			case 'mimeType':
				const allowedTypes = this.allowedMimeType.join();               
				this.errorMessage = `Esse tipo de arquivo: "${item.type} não é aceito para envio.\n Permitido são: "${allowedTypes}"`;
			break;

			default:
				this.errorMessage = `Erro desconhecido (filter is ${filter.name})`;
      }
      //this.commonService.toastr.warning(this.errorMessage);      
      this.onLimparQueue();      
    }
}