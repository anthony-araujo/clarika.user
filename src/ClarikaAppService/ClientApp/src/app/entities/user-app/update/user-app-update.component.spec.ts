import { ComponentFixture, TestBed } from "@angular/core/testing";
import { HttpResponse } from "@angular/common/http";
import { HttpClientTestingModule } from "@angular/common/http/testing";
import { FormBuilder } from "@angular/forms";
import { ActivatedRoute } from "@angular/router";
import { RouterTestingModule } from "@angular/router/testing";
import { of, Subject, from } from "rxjs";

import { UserAppService } from "../service/user-app.service";
import { IUserApp, UserApp } from "../user-app.model";
import { ICountry } from "app/entities/country/country.model";
import { CountryService } from "app/entities/country/service/country.service";

import { UserAppUpdateComponent } from "./user-app-update.component";

describe("UserApp Management Update Component", () => {
  let comp: UserAppUpdateComponent;
  let fixture: ComponentFixture<UserAppUpdateComponent>;
  let activatedRoute: ActivatedRoute;
  let userAppService: UserAppService;
  let countryService: CountryService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, RouterTestingModule.withRoutes([])],
      declarations: [UserAppUpdateComponent],
      providers: [
        FormBuilder,
        {
          provide: ActivatedRoute,
          useValue: {
            params: from([{}]),
          },
        },
      ],
    })
      .overrideTemplate(UserAppUpdateComponent, "")
      .compileComponents();

    fixture = TestBed.createComponent(UserAppUpdateComponent);
    activatedRoute = TestBed.inject(ActivatedRoute);
    userAppService = TestBed.inject(UserAppService);
    countryService = TestBed.inject(CountryService);

    comp = fixture.componentInstance;
  });

  describe("ngOnInit", () => {
    it("Should call Country query and add missing value", () => {
      const userApp: IUserApp = { id: 456 };
      const country: ICountry = { id: 16940 };
      userApp.country = country;

      const countryCollection: ICountry[] = [{ id: 54724 }];
      jest
        .spyOn(countryService, "query")
        .mockReturnValue(of(new HttpResponse({ body: countryCollection })));
      const additionalCountries = [country];
      const expectedCollection: ICountry[] = [
        ...additionalCountries,
        ...countryCollection,
      ];
      jest
        .spyOn(countryService, "addCountryToCollectionIfMissing")
        .mockReturnValue(expectedCollection);

      activatedRoute.data = of({ userApp });
      comp.ngOnInit();

      expect(countryService.query).toHaveBeenCalled();
      expect(
        countryService.addCountryToCollectionIfMissing
      ).toHaveBeenCalledWith(countryCollection, ...additionalCountries);
      expect(comp.countriesSharedCollection).toEqual(expectedCollection);
    });

    it("Should update editForm", () => {
      const userApp: IUserApp = { id: 456 };
      const country: ICountry = { id: 52311 };
      userApp.country = country;

      activatedRoute.data = of({ userApp });
      comp.ngOnInit();

      expect(comp.editForm.value).toEqual(expect.objectContaining(userApp));
      expect(comp.countriesSharedCollection).toContain(country);
    });
  });

  describe("save", () => {
    it("Should call update service on save for existing entity", () => {
      // GIVEN
      const saveSubject = new Subject<HttpResponse<UserApp>>();
      const userApp = { id: 123 };
      jest.spyOn(userAppService, "update").mockReturnValue(saveSubject);
      jest.spyOn(comp, "previousState");
      activatedRoute.data = of({ userApp });
      comp.ngOnInit();

      // WHEN
      comp.save();
      expect(comp.isSaving).toEqual(true);
      saveSubject.next(new HttpResponse({ body: userApp }));
      saveSubject.complete();

      // THEN
      expect(comp.previousState).toHaveBeenCalled();
      expect(userAppService.update).toHaveBeenCalledWith(userApp);
      expect(comp.isSaving).toEqual(false);
    });

    it("Should call create service on save for new entity", () => {
      // GIVEN
      const saveSubject = new Subject<HttpResponse<UserApp>>();
      const userApp = new UserApp();
      jest.spyOn(userAppService, "create").mockReturnValue(saveSubject);
      jest.spyOn(comp, "previousState");
      activatedRoute.data = of({ userApp });
      comp.ngOnInit();

      // WHEN
      comp.save();
      expect(comp.isSaving).toEqual(true);
      saveSubject.next(new HttpResponse({ body: userApp }));
      saveSubject.complete();

      // THEN
      expect(userAppService.create).toHaveBeenCalledWith(userApp);
      expect(comp.isSaving).toEqual(false);
      expect(comp.previousState).toHaveBeenCalled();
    });

    it("Should set isSaving to false on error", () => {
      // GIVEN
      const saveSubject = new Subject<HttpResponse<UserApp>>();
      const userApp = { id: 123 };
      jest.spyOn(userAppService, "update").mockReturnValue(saveSubject);
      jest.spyOn(comp, "previousState");
      activatedRoute.data = of({ userApp });
      comp.ngOnInit();

      // WHEN
      comp.save();
      expect(comp.isSaving).toEqual(true);
      saveSubject.error("This is an error!");

      // THEN
      expect(userAppService.update).toHaveBeenCalledWith(userApp);
      expect(comp.isSaving).toEqual(false);
      expect(comp.previousState).not.toHaveBeenCalled();
    });
  });

  describe("Tracking relationships identifiers", () => {
    describe("trackCountryById", () => {
      it("Should return tracked Country primary key", () => {
        const entity = { id: 123 };
        const trackResult = comp.trackCountryById(0, entity);
        expect(trackResult).toEqual(entity.id);
      });
    });
  });
});
