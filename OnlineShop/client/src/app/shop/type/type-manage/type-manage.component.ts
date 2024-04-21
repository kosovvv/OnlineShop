import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Type } from 'src/app/shared/models/type';
import { TypeService } from 'src/app/core/services/type-service';

@Component({
  selector: 'app-type-manage',
  templateUrl: './type-manage.component.html',
  styleUrls: ['./type-manage.component.scss']
})

export class TypeManageComponent implements OnInit{
  fileName: string | null = null;
  image: File | null = null;
  errors: string[] | null = null;
  types: Type[] = [];

  typeForm = this.fb.group({
    name: ['', Validators.required],
    pictureUrl: ['', Validators.required],
  })

  constructor(private fb: FormBuilder,private typeService: TypeService, private router: Router, private toastr: ToastrService) {}
  
  ngOnInit(): void {
    this.getTypes();
  }

  getTypes() {
    return this.typeService.getTypes().subscribe({
      next: (types) => {
        if (types) {
          this.types = types;
        }
      }
    })
  }

  deleteType(typeId : number) {
    this.typeService.deleteType(typeId).subscribe({
      next: () => {
        this.getTypes();
      }
    })
  }

}
