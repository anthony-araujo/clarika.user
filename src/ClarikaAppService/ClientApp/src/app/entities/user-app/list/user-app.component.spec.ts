import { ComponentFixture, TestBed } from "@angular/core/testing";
import { HttpHeaders, HttpResponse } from "@angular/common/http";
import { HttpClientTestingModule } from "@angular/common/http/testing";
import { of } from "rxjs";

import { UserAppService } from "../service/user-app.service";

import { UserAppComponent } from "./user-app.component";

describe("UserApp Management Component", () => {
  let comp: UserAppComponent;
  let fixture: ComponentFixture<UserAppComponent>;
  let service: UserAppService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      declarations: [UserAppComponent],
    })
      .overrideTemplate(UserAppComponent, "")
      .compileComponents();

    fixture = TestBed.createComponent(UserAppComponent);
    comp = fixture.componentInstance;
    service = TestBed.inject(UserAppService);

    const headers = new HttpHeaders();
    jest.spyOn(service, "query").mockReturnValue(
      of(
        new HttpResponse({
          body: [{ id: 123 }],
          headers,
        })
      )
    );
  });

  it("Should call load all on init", () => {
    // WHEN
    comp.ngOnInit();

    // THEN
    expect(service.query).toHaveBeenCalled();
    expect(comp.userApps?.[0]).toEqual(expect.objectContaining({ id: 123 }));
  });
});
