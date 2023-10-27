import { Component, OnInit } from "@angular/core";
import { HttpResponse } from "@angular/common/http";
import { FormBuilder, Validators } from "@angular/forms";
import { ActivatedRoute } from "@angular/router";
import { Observable } from "rxjs";
import { finalize, map } from "rxjs/operators";

import { IUserApp, UserApp } from "../user-app.model";
import { UserAppService } from "../service/user-app.service";
import { ICountry } from "app/entities/country/country.model";
import { CountryService } from "app/entities/country/service/country.service";

@Component({
  selector: "jhi-user-app-update",
  templateUrl: "./user-app-update.component.html",
})
export class UserAppUpdateComponent implements OnInit {
  isSaving = false;

  countriesSharedCollection: ICountry[] = [];

  editForm = this.fb.group({
    id: [],
    firstName: [null, [Validators.required]],
    lastName: [null, [Validators.required]],
    email: [],
    dateBirth: [],
    age: [null, [Validators.min(0), Validators.max(150)]],
    passwordHash: [],
    securityStamp: [],
    concurrencyStamp: [],
    country: [],
  });

  constructor(
    protected userAppService: UserAppService,
    protected countryService: CountryService,
    protected activatedRoute: ActivatedRoute,
    protected fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.activatedRoute.data.subscribe(({ userApp }) => {
      this.updateForm(userApp);

      this.loadRelationshipsOptions();
    });
  }

  previousState(): void {
    window.history.back();
  }

  save(): void {
    this.isSaving = true;
    const userApp = this.createFromForm();
    if (userApp.id !== undefined) {
      this.subscribeToSaveResponse(this.userAppService.update(userApp));
    } else {
      this.subscribeToSaveResponse(this.userAppService.create(userApp));
    }
  }

  trackCountryById(_index: number, item: ICountry): number {
    return item.id!;
  }

  protected subscribeToSaveResponse(
    result: Observable<HttpResponse<IUserApp>>
  ): void {
    result.pipe(finalize(() => this.onSaveFinalize())).subscribe({
      next: () => this.onSaveSuccess(),
      error: () => this.onSaveError(),
    });
  }

  protected onSaveSuccess(): void {
    this.previousState();
  }

  protected onSaveError(): void {
    // Api for inheritance.
  }

  protected onSaveFinalize(): void {
    this.isSaving = false;
  }

  protected updateForm(userApp: IUserApp): void {
    this.editForm.patchValue({
      id: userApp.id,
      firstName: userApp.firstName,
      lastName: userApp.lastName,
      email: userApp.email,
      dateBirth: userApp.dateBirth,
      age: userApp.age,
      passwordHash: userApp.passwordHash,
      securityStamp: userApp.securityStamp,
      concurrencyStamp: userApp.concurrencyStamp,
      country: userApp.country,
    });

    this.countriesSharedCollection =
      this.countryService.addCountryToCollectionIfMissing(
        this.countriesSharedCollection,
        userApp.country
      );
  }

  protected loadRelationshipsOptions(): void {
    this.countryService
      .query()
      .pipe(map((res: HttpResponse<ICountry[]>) => res.body ?? []))
      .pipe(
        map((countries: ICountry[]) =>
          this.countryService.addCountryToCollectionIfMissing(
            countries,
            this.editForm.get("country")!.value
          )
        )
      )
      .subscribe(
        (countries: ICountry[]) => (this.countriesSharedCollection = countries)
      );
  }

  protected createFromForm(): IUserApp {
    return {
      ...new UserApp(),
      id: this.editForm.get(["id"])!.value,
      firstName: this.editForm.get(["firstName"])!.value,
      lastName: this.editForm.get(["lastName"])!.value,
      email: this.editForm.get(["email"])!.value,
      dateBirth: this.editForm.get(["dateBirth"])!.value,
      age: this.editForm.get(["age"])!.value,
      passwordHash: this.editForm.get(["passwordHash"])!.value,
      securityStamp: this.editForm.get(["securityStamp"])!.value,
      concurrencyStamp: this.editForm.get(["concurrencyStamp"])!.value,
      country: this.editForm.get(["country"])!.value,
    };
  }
}
